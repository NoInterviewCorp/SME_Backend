using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Core.Connections;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.Models;
namespace SME.Services
{
    public class DatabaseCommunicator : IDatabaseCommunicator
    {
        private MongoDbConnection db;
        private RabbitMQConnection rabbit;
        
        public DatabaseCommunicator(MongoDbConnection db, RabbitMQConnection rabbit)
        {
            this.db = db;
            this.rabbit = rabbit;
            HandleQuestionRequestFromQueue();
            HandleResourceRequestFromQueue();
        }
        public QuestionBatchResponse ProvideQuestionsFromId(QuestionBatchRequest batchRequest)
        {
            var response = new List<Question>();
            foreach (var request in batchRequest.IdRequestList)
            {
                var result = db.Questions.Find(q => q.QuestionId == request).SingleOrDefault();
                if (result == null)
                {
                    throw new Exception($"Question with the QuestionId {request} does not exist inside SME MongoDB");
                }
                response.Add(result);
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
                try
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                    var body = ea.Body;
                    var request = (QuestionBatchRequest)body.DeSerialize(typeof(QuestionBatchRequest));
                    Console.WriteLine("Username " + request.Username + " is requesting " + request.IdRequestList.Count + " Questions");
                    var qbr = ProvideQuestionsFromId(request);
                    var response = ObjectSerialize.Serialize(qbr);
                    Console.WriteLine("Questions requested are->");
                    foreach (var item in qbr.ResponseList)
                    {
                        Console.WriteLine(JsonConvert.SerializeObject(item));
                    }
                    Console.WriteLine("Sending " + qbr.ResponseList.Count + " Questions to Quiz Engine ");

                    // Send a message back to QuizEngine with the necessary question as response
                    rabbit.Model.BasicPublish(
                                exchange: rabbit.ExchangeName,
                                routingKey: "Send.Question",
                                basicProperties: null,
                                body: response
                            );
                    Console.WriteLine("Published to Question Response QuizEngine");
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    ConsoleWriter.ConsoleAnException(e);
                }
            };
            Console.WriteLine("Listening to Knowledge Graph microservice for Question ID request ");
            channel.BasicConsume("KnowledgeGraph_Contributer_Ids", false, consumer);
        }

        public List<Resource> ProvideRecommendedResources(List<string> resourceIds)
        {
            var filterDefinition = Builders<Resource>.Filter.In(r => r.ResourceId, resourceIds);
            var plans = db.Resources.Find(filterDefinition).ToList();
            Console.WriteLine(plans.Count+" Resources have been sent");
            return plans;
        }

        public void HandleResourceRequestFromQueue()
        {
            var channel = rabbit.Connection.CreateModel();
            var consumer = new AsyncEventingBasicConsumer(channel);
            Console.WriteLine("----------------------------------------------------------------");
            consumer.Received += async (model, ea) =>
            {
                Console.WriteLine("-----------------------------------------------------------------------");
                Console.WriteLine("Consuming from KnowledgeGraph ");
                try
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                    var body = ea.Body;
                    var request = (List<string>)body.DeSerialize(typeof(List<string>));
                    Console.WriteLine("Request of "+request.Count+" Resource Ids");
                    var qbr = ProvideRecommendedResources(request);
                    var response = ObjectSerialize.Serialize(qbr);
                    Console.WriteLine("Resources requested are->");
                    foreach (var item in qbr)
                    {
                        Console.WriteLine(JsonConvert.SerializeObject(item));
                    }
                    Console.WriteLine("Sending " + qbr.Count + " Resources to Quiz Engine ");

                    // Send a message back to QuizEngine with the necessary question as response
                    rabbit.Model.BasicPublish(
                                exchange: rabbit.ExchangeName,
                                routingKey: "Consume.Resource",
                                basicProperties: null,
                                body: response
                            );
                    Console.WriteLine("Published to Resource -> QuizEngine");
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    ConsoleWriter.ConsoleAnException(e);
                }
            };
            Console.WriteLine("Listening to Knowledge Graph microservice for Resource ID request ");
            channel.BasicConsume("KnowledgeGraph_Contributer_ResourceIds", false, consumer);
        }
    }
}