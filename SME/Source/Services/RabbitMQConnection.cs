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
        // private MongoDbConnection dbConnection;

        public RabbitMQConnection()
        {
            // this.dbConnection = dbConnection;

            Factory = new ConnectionFactory
            {
                HostName = "localhost",
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
            // Model.QueueDeclare("QuizEngine_KnowledgeGraph_Query", false, false, false, null);
            // Model.QueueDeclare("QuizEngine_KnowledgeGraph_QuizUpdate", false, false, false, null);
            // Model.QueueDeclare("QuizEngine_UserProfile_UserData", false, false, false, null);
        }
        // public void GetQuestions (byte[] message, string RoutingKey) {
        //     IBasicProperties props = _model.CreateBasicProperties ();
        //     props.Expiration = "10000";
        //     _model.BasicPublish (ExchangeNme, RoutingKey, props, message);
        // }
        // public void FetchQuestions () {
        //     var channel = connection.CreateModel ();
        //     var consumer = new AsyncEventingBasicConsumer (channel);
        //     consumer.Received += async (model, ea) => {
        //         var body = ea.Body;
        //         var json = Encoding.Default.GetString (body);
        //         questions.Clear ();
        //         questions.AddRange (JsonConvert.DeserializeObject<List<Question>> (json));
        //         var routingKey = ea.RoutingKey;
        //         Console.WriteLine (" - Routing Key <{0}>", routingKey);
        //         channel.BasicAck (ea.DeliveryTag, false);
        //         await Task.Yield ();
        //     };
        // }
    }
}
