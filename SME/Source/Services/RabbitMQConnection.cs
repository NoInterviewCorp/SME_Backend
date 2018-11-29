using RabbitMQ.Client;

namespace SME.Services
{
    public class RabbitMQConnection
    {
        private ConnectionFactory Factory;
        public IConnection Connection { get; set; }
        public IModel Model { get; set; }
        public string ExchangeNme
        {
            get { return "KnowldegeGraphExchange"; }
        }


        public RabbitMQConnection()
        {
            Factory = new ConnectionFactory
            {
                HostName = "172.23.238.173",
                // Port = 8080,
                UserName = "achausername",
                Password = "strongpassword",
                DispatchConsumersAsync = true
            };
            Connection = Factory.CreateConnection();
            Model = Connection.CreateModel();
            Model.ExchangeDeclare("KnowldegeGraphExchange", "topic");
            Model.QueueDeclare("Contributer_KnowledgeGraph", false, false, false, null);
            Model.QueueBind("Contributer_KnowledgeGraph", ExchangeNme, "Models.LearningPlan");
            // _model.QueueDeclare ("QuizEngine_KnowledgeGraph", false, false, false, null);
            // _model.QueueDeclare ("KnowledgeGraph_IDs", false, false, false, null);
            // _model.QueueDeclare ("Contributer_Questions", false, false, false, null);
            // _model.QueueBind ("QuizEngine_KnowledgeGraph", ExchangeNme, "Models.Technology");
            // _model.QueueBind ("KnowledgeGraph_IDs", ExchangeNme, "Models.QuestionId");
            // _model.QueueBind ("Contributer_Questions", ExchangeNme, "Models.Queation");
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