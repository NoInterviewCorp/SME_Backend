using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.Services
{
    public class RabbitMQConnection
    {
        private ConnectionFactory Factory;
        public IConnection Connection { get; set; }
        public IModel Model { get; set; }
        private AsyncEventingBasicConsumer consumer;
        private IBasicProperties properties;
        private BlockingCollection<List<LearningPlanInfo>> responseQueue = new BlockingCollection<List<LearningPlanInfo>>();
        private BlockingCollection<List<LearningPlanInfo>> popularPlansQueue = new BlockingCollection<List<LearningPlanInfo>>();
        private BlockingCollection<List<LearningPlanInfo>> subscriptionsQueue = new BlockingCollection<List<LearningPlanInfo>>();
        public string ExchangeName = "KnowledgeGraphExchange";
        private string replyQueueName = "AverageRating_TotalSubs_Response";
        public RabbitMQConnection(IOptions<RabbitMQSettings> options)
        {
            var settings = options.Value;
            Factory = new ConnectionFactory
            {
                HostName = settings.ConnectionString,
                UserName = settings.Username,
                Password = settings.Password,
                DispatchConsumersAsync = true
            };
            try
            {
                Connection = Factory.CreateConnection();
                Model = Connection.CreateModel();
                Model.ExchangeDeclare(ExchangeName, "topic");
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("--------------------------------------------------------------");
            }
        }

        public List<LearningPlanInfo> GetLearningPlanInfo(List<string> learningPlanIds)
        {
            // Initializing the connection
            consumer = new AsyncEventingBasicConsumer(Model);
            properties = Model.CreateBasicProperties();
            var correlationId = " 1" ;//Guid.NewGuid().ToString("N");
            properties.CorrelationId = correlationId;
            properties.ReplyTo = "Response.LP";

            // Initialising the reciever
            consumer.Received += async (model, ea) =>
            {
                Console.WriteLine("Response Recieved LearningPlan Info");
                try
                {
                    var body = ea.Body;
                    var response = (List<LearningPlanInfo>)body.DeSerialize(typeof(List<LearningPlanInfo>));
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        Console.WriteLine($"Adding reponse to the queue with {response.Count} objects");
                        responseQueue.Add(response);
                    }
                }
                catch (Exception e)
                {
                    ConsoleWriter.ConsoleAnException(e);
                }
                await Task.Yield();
            };

            // Preparing message and publishing it
            Console.WriteLine($"Sending Request for {learningPlanIds.Count} Ids");
            var messageBytes = learningPlanIds.Serialize();
            Model.BasicPublish(
                exchange: ExchangeName,
                routingKey: "Request.LP",
                basicProperties: properties,
                body: messageBytes);

            // Starting to listen for the response
            Model.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);
            Console.Write("Taking from BlockingCollection with count " + responseQueue.Count);
            return responseQueue.Take();
        }

        public List<LearningPlanInfo> GetPopularPlans(string techName)
        {
            // Initializing the connection
            var consumer2 = new AsyncEventingBasicConsumer(Model);
            var properties = Model.CreateBasicProperties();
            var correlationId = " 1"; // Guid.NewGuid().ToString("N");
            properties.CorrelationId = correlationId;
            properties.ReplyTo = "Reponse.PopularPlans";

            // Initialising the reciever
            consumer2.Received += async (model, ea) =>
            {
                Console.WriteLine("Response Recieved for Popular plans");
                try
                {
                    var body = ea.Body;
                    var response = (List<LearningPlanInfo>)body.DeSerialize(typeof(List<LearningPlanInfo>));
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        Console.WriteLine($"Adding response to the queue with {response.Count} objects");
                        popularPlansQueue.Add(response);
                    }
                }
                catch (Exception e)
                {
                    ConsoleWriter.ConsoleAnException(e);
                }
                await Task.Yield();
            };

            // Preparing message and publishing it
            Console.WriteLine($"Sending Request for Popular plans for {techName}");
            var messageBytes = techName.Serialize();
            Model.BasicPublish(
                exchange: ExchangeName,
                routingKey: "Request.PopularPlans",
                basicProperties: properties,
                body: messageBytes);

            // Starting to listen for the response
            Model.BasicConsume(
                consumer: consumer2,
                queue: "KnowledgeGraph_Contributer_PopularPlans",
                autoAck: true);
            Console.Write("Taking from Popular Plans Queue with count " + responseQueue.Count);
            return popularPlansQueue.Take();
        }

        public List<LearningPlanInfo> GetSubscriptions(string user)
        {
            // Initializing the connection
            var consumer3 = new AsyncEventingBasicConsumer(Model);
            var properties = Model.CreateBasicProperties();
            var correlationId = " 1"; // Guid.NewGuid().ToString("N");
            properties.CorrelationId = correlationId;
            properties.ReplyTo = "Reponse.Subscriptions";

            // Initialising the reciever
            consumer3.Received += async (model, ea) =>
            {
                Console.WriteLine("Response Recieved for Subscriptions of user -> "+user);
                try
                {
                    var body = ea.Body;
                    var response = (List<LearningPlanInfo>)body.DeSerialize(typeof(List<LearningPlanInfo>));
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        Console.WriteLine($"Adding reponse to the queue with {response.Count} objects");
                        subscriptionsQueue.Add(response);
                    }
                }
                catch (Exception e)
                {
                    ConsoleWriter.ConsoleAnException(e);
                }
                await Task.Yield();
            };

            // Preparing message and publishing it
            Console.WriteLine($"Sending Request for Subscription for {user}");
            var messageBytes = user.Serialize();
            Model.BasicPublish(
                exchange: ExchangeName,
                routingKey: "Request.Subscriptions",
                basicProperties: properties,
                body: messageBytes);

            // Starting to listen for the response
            Model.BasicConsume(
                consumer: consumer3,
                queue: "KnowledgeGraph_Contributer_Subscriptions",
                autoAck: true);
            Console.Write("Taking from Popular Plans Queue with count " + responseQueue.Count);
            return subscriptionsQueue.Take();
        }

        public void Close()
        {
            Connection.Close();
            responseQueue.Dispose();
        }
    }
}
