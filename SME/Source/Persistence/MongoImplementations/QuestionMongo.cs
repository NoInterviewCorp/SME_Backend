// using System.Collections.Generic;
// using System.Threading.Tasks;
// using SME.Models;
// using SME.Services;
// using MongoDB.Driver;

// namespace SME.Persistence{
//     public class QuestionMongo : IQuestionRepository
//     {
//         private MongoDbConnection dbConnection;
//         public QuestionMongo(MongoDbConnection dbConnection)
//         {
//             this.dbConnection = dbConnection;
//         }
//         public async Task<List<Question>> AddQuestionsAsync(List<Question> question)
//         {
//             throw new System.NotImplementedException();
//         }

//         public async Task<bool> DeleteQuestionByIdAsync(string questionId)
//         {
//             var removeQuery = await dbConnection.Questions.DeleteOneAsync(q => q.QuestionId == questionId);
//             if (removeQuery.DeletedCount > 0)
//             {
//                 return true;
//             }
//             else
//             {
//                 return false;
//             }
//         }

//         public async Task<List<Question>> GetQuestionsAsync()
//         {
//             var plans = await dbConnection.Questions
//                             .Find(new FilterDefinitionBuilder<Question>().Empty)
//                             .ToListAsync();
//             return (plans.Count > 0) ? plans : null;
//         }

//         public async Task<List<Question>> GetQuestionsByConceptOfATechAsync(string technology, string concept)
//         {
//             throw new System.NotImplementedException();
//         }

//         public async Task<Question> UpdateQuestionAsync(Question question)
//         {
//             throw new System.NotImplementedException();
//         }
//     }
// }