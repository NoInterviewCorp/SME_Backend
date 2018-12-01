using RabbitMQ.Client;
using System;

namespace SME.Services
{
    public class RabbitMQConnection
    {
        private ConnectionFactory Factory;
        public IConnection Connection { get; set; }
        public IModel Model { get; set; }
        public string ExchangeNme = "KnowldegeGraphExchange";
        public RabbitMQConnection()
        {
            // this.dbConnection = dbConnection;

            Factory = new ConnectionFactory
            {
                HostName = "",
                // Port = 8080,
                UserName = "achausername",
                Password = "strongpassword",
                DispatchConsumersAsync = true
            };
            try
            {
                Connection = Factory.CreateConnection();
                Model = Connection.CreateModel();
                Model.ExchangeDeclare("KnowldegeGraphExchange", "topic");
                Model.QueueDeclare("Contributer_KnowledgeGraph_LearningPlan", false, false, false, null);
                Model.QueueDeclare("Contributer_KnowledgeGraph_Resources", false, false, false, null);
                Model.QueueDeclare("Contributer_QuizEngine_Questions", false, false, false, null);
                Model.QueueDeclare("KnowledgeGraph_Contributer_Ids", false, false, false, null);
                Model.QueueBind("Contributer_KnowledgeGraph_LearningPlan", ExchangeNme, "Models.LearningPlan");
                Model.QueueBind("Contributer_KnowledgeGraph_Resources", ExchangeNme, "Models.Resource");
                Model.QueueBind("Contributer_QuizEngine_Questions", ExchangeNme, "Send.Question");
                Model.QueueBind("KnowledgeGraph_Contributer_Ids", ExchangeNme, "Request.Question");
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
