using System.Collections.Generic;
using System.Threading.Tasks;
using SME.Models;
using SME.Services;
using MongoDB.Driver;
using System.Linq;
using System;
using System.Text;

namespace SME.Persistence
{
    public class QuestionMongo : IQuestionRepository
    {
        private MongoDbConnection dbConnection;
        public QuestionMongo(MongoDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }
        public async Task<List<Question>> AddQuestionsAsync(List<Question> questions, string resourceId)
        {
            // Adding resource Id to quesitons and changing case of 
            // every concept and technology to upper case
            questions = questions
                .Select(q => {
                    q.ResourceId = resourceId; 
                    q.Concepts = q.Concepts
                        .Select(c=>{
                            c.Name = c.Name.ToUpper();
                            return c;
                        })
                        .ToList();
                    q.Technology.Name = q.Technology.Name.ToUpper();
                    return q; 
                })
                .ToList();
            var filter = Builders<Resource>.Filter.Where(r => r.ResourceId == resourceId);
            var resourceUpdateDefinition = Builders<Resource>.Update
                .PushEach(r => r.Questions, questions);
            var resourceUpdate = dbConnection.Resources.FindOneAndUpdateAsync(
                filter,
                resourceUpdateDefinition,
                new FindOneAndUpdateOptions<Resource>() { IsUpsert = true }
            );
            var questionsInsert = dbConnection.Questions.InsertManyAsync(questions);
            await questionsInsert;
            await resourceUpdate;
            return questions;
        }

        public async Task<bool> DeleteQuestionByIdAsync(string questionId)
        {
            var removeQuery = await dbConnection.Questions.DeleteOneAsync(q => q.QuestionId == questionId);
            if (removeQuery.DeletedCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Question>> GetQuestionsAsync()
        {
            var plans = await dbConnection.Questions
                            .Find(new FilterDefinitionBuilder<Question>().Empty)
                            .ToListAsync();
            return (plans.Count > 0) ? plans : null;
        }

        public async Task<List<Question>> GetQuestionsByResourceAsync(string resourceId)
        {
            var questions = await dbConnection.Questions
                            .Find(q => q.ResourceId == resourceId)
                            .ToListAsync();
            return (questions.Count > 0) ? questions : null;
        }

        public async Task<Question> UpdateQuestionAsync(Question question)
        {
            await Task.Yield();
            throw new System.NotImplementedException();
        }
    }
}