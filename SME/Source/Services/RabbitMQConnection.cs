using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.Models;
using System;
using System.Collections.Concurrent;
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
        private BlockingCollection<LearningPlanInfo> responseQueue = new BlockingCollection<LearningPlanInfo>();
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

        public LearningPlanInfo GetLearningPlanInfo(string learningPlanId)
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
                var body = ea.Body;
                var response = (LearningPlanInfo)body.DeSerialize(typeof(LearningPlanInfo));
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    responseQueue.Add(response);
                }
                await Task.Yield();
            };

            // Preparing message and publishing it
            var messageBytes = Encoding.UTF8.GetBytes(learningPlanId);
            Model.BasicPublish(
                exchange: "",
                routingKey: "Request.LP",
                basicProperties: properties,
                body: messageBytes);

            // Starting to listen for the response
            Model.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            return responseQueue.Take();
        }
        public void Close()
        {
            Connection.Close();
            responseQueue.Dispose();
        }
    }
}
