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
                Model.QueueDeclare("Contributer_KnowledgeGraph_LearningPlan", false, false, false, null);
                Model.QueueDeclare("Contributer_KnowledgeGraph_Resources", false, false, false, null);
                Model.QueueDeclare("Contributer_QuizEngine_Questions", false, false, false, null);
                Model.QueueDeclare("KnowledgeGraph_Contributer_Ids", false, false, false, null);
                Model.QueueBind("Contributer_KnowledgeGraph_LearningPlan", ExchangeName, "Models.LearningPlan");
                Model.QueueBind("Contributer_KnowledgeGraph_Resources", ExchangeName, "Models.Resource");
                Model.QueueBind("Contributer_QuizEngine_Questions", ExchangeName, "Send.Question");
                Model.QueueBind("KnowledgeGraph_Contributer_Ids", ExchangeName, "Request.Question");
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
