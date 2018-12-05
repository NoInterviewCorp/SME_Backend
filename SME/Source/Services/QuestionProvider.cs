using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.Models;

namespace SME.Services
{
    public class QuestionProvider
    {
        // public void ConsumeQuestionRequest(IConnection connection)
        // {
        //     var channel = connection.CreateModel();
        //     var consumer = new AsyncEventingBasicConsumer(channel);
        //     consumer.Received += async (model, ea) =>
        //     {
        //         Console.WriteLine("Consuming QuestionIds from the queue");
        //         Console.WriteLine("-----------------------------------------------------------------------");
        //         var body = ea.Body;
        //         var questionBatch = (QuestionBatchRequest) body.DeSerialize(typeof(QuestionBatchRequest));
        //         var response = await GetQuestionsFromIdsAsync(questionBatch);
        //         var routingKey = ea.RoutingKey;
        //         channel.BasicAck(ea.DeliveryTag, false);
        //         Console.WriteLine("-----------------------------------------------------------------------");
        //         Console.WriteLine(" - Routing Key <{0}>", routingKey);
        //         await Task.Yield();
        //     };
        //     Console.WriteLine("Consuming QuestionId Request from Knowledge Graph");
        //     channel.BasicConsume("KnowledgeGraph_Contributer_Ids", false, consumer);
        // }

        // private async Task<QuestionBatchResponse> GetQuestionsFromIdsAsync(QuestionBatchRequest questionBatch)
        // {
        //     foreach(var request in questionBatch.IRequestList){

        //     }
        //     await Task.Yield();
        //     return null;
        // }
    }
}