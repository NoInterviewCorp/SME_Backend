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
    public class QuestionRequestHandler : IQuestionRequestHandler
    {
        private MongoDbConnection db;
        private RabbitMQConnection rabbit;
        public QuestionRequestHandler(MongoDbConnection db,RabbitMQConnection rabbit)
        {
            this.db = db;
            this.rabbit = rabbit;
            HandleQuestionRequestFromQueue();
        }
        public QuestionBatchResponse ProvideQuestionsFromId(QuestionBatchRequest batchRequest)
        {
            var response = new Dictionary<string, List<Question>>();
            foreach (var request in batchRequest.IdRequestDictionary)
            {
                // figure out how to pass an array here to query all documents
                // var filter = "{ QuestionId: { $in: " + request.Value.ToArray() + " } }";
                // var result = db.Questions.FindSync(filter).ToList();
                var result = db.Questions.Find(q=>request.Value.Find(q2=>q2 == q.QuestionId)!= null).ToList();
                response.Add(request.Key, result);
            }
            return new QuestionBatchResponse(batchRequest.Username, response);
        }

        public void HandleQuestionRequestFromQueue()
        {
            var channel = rabbit.Connection.CreateModel();
            var consumer = new AsyncEventingBasicConsumer(channel);
            Console.WriteLine("----------------------------------------------------------------");
            consumer.Received += async (model, ea) =>
            {
                Console.WriteLine("-----------------------------------------------------------------------");
                Console.WriteLine("Consuming from KnowledgeGraph ");
                channel.BasicAck(ea.DeliveryTag, false);
                var body = ea.Body;
                var request = (QuestionBatchRequest)body.DeSerialize(typeof(QuestionBatchRequest));
                Console.WriteLine("Request Username is " + request.Username);
                var routingKey = ea.RoutingKey;
                Console.WriteLine("-----------------------------------------------------------------------");
                Console.WriteLine(" - Routing Key <{0}>", routingKey);
                var qbr = ProvideQuestionsFromId(request);
                Console.WriteLine("Reponse Username is "+ qbr.Username);
                var response = ObjectSerialize.Serialize(qbr);
                Console.WriteLine("Sending Questions to Quiz Engine ");
                // Send a message back to QuizEngine with the necessary question as response
                rabbit.Model.BasicPublish(
                            exchange: rabbit.ExchangeName,
                            routingKey: "Send.Question",
                            basicProperties: null,
                            body: response
                        );
                Console.WriteLine("Publishing to Question Response QuizEngine");
                await Task.Yield();
            };
            Console.WriteLine("Listening to Knowledge Graph microservice for Question ID request ");
            channel.BasicConsume("KnowledgeGraph_Contributer_Ids", false, consumer);
        }
    }
}