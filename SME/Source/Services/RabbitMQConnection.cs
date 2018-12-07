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
            var correlationId = Guid.NewGuid().ToString("N");
            properties.CorrelationId = correlationId;
            properties.ReplyTo = replyQueueName;

            // Initialising the reciever
            consumer.Received += async (model, ea) =>
            {
                Console.WriteLine("Response Recieved");
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
            Console.Write("Taking from BlockingCollection");
            return responseQueue.Take();
        }
        public void Close()
        {
            Connection.Close();
            responseQueue.Dispose();
        }
    }
}
