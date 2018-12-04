using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;

namespace SME.Services
{
    public class RabbitMQConnection
    {
        private ConnectionFactory Factory;
        public IConnection Connection { get; set; }
        public IModel Model { get; set; }
        public string ExchangeName = "KnowldegeGraphExchange";
        public RabbitMQConnection(IOptions<RabbitMQSettings> options)
        {
            // this.dbConnection = dbConnection;
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
                Model.QueueDeclare("Contributer_QuizEngine_Questions", false, false, false, null);
                Model.QueueBind("Contributer_QuizEngine_Questions", ExchangeName, "Send.Question");
            }
            catch (Exception e)
            {
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("--------------------------------------------------------------");
            }
        }
    }
}
