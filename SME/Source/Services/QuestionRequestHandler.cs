using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.Models;
namespace SME.Services
{
    public class QuestionRequestHandler
    {
        private MongoDbConnection db;
        private RabbitMQConnection rabbit;
        public QuestionRequestHandler(MongoDbConnection db,RabbitMQConnection rabbit)
        {
            this.db = db;
            this.rabbit = rabbit;
        }
        public QuestionBatchResponse ProvideQuestionsFromId(QuestionBatchRequest batchRequest)
        {
            var response = new Dictionary<string, List<Question>>();
            foreach (var request in batchRequest.RequestDictionary)
            {
                // figure out how to pass an array here to query all documents
                var filter = "{ QuestionId: { $in: " + request.Value.ToArray() + " } }";
                var result = db.Questions.FindSync(filter).ToList();
                response.Add(request.Key, result);
            }
            return new QuestionBatchResponse(batchRequest.Username, response);
        }

        public void HandleLearningPlanFromQueue()
        {
            var channel = rabbit.Connection.CreateModel();
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                Console.WriteLine("-----------------------------------------------------------------------");
                Console.WriteLine("Consuming from KnowledgeGraph ");
                var body = ea.Body;
                var request = (QuestionBatchRequest)body.DeSerialize(typeof(QuestionBatchRequest));
                var routingKey = ea.RoutingKey;
                channel.BasicAck(ea.DeliveryTag, false);
                Console.WriteLine("-----------------------------------------------------------------------");
                Console.WriteLine(" - Routing Key <{0}>", routingKey);
                var response = ObjectSerialize.Serialize(ProvideQuestionsFromId(request));
                // Send a message back to QuizEngine with the necessary question as response
                rabbit.Model.BasicPublish(
                            exchange: rabbit.ExchangeName,
                            routingKey: "QuestionResponse",
                            basicProperties: null,
                            body: response
                        );
                Console.WriteLine("Publishing to Question Response QuizEngine");
                await Task.Yield();
            };
            Console.WriteLine("Listening to Knowledge Graph microservice ");
            channel.BasicConsume("KnowledgeGraph_Contributer_Ids", false, consumer);
        }
    }
}