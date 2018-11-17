using SME.Models;
using SME.Services;
using Neo4jClient;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Dynamic;

namespace SME.Persistence
{
    public class QuestionRepository : IQuestionRepository
    {
        private GraphClient graph;
        public QuestionRepository(GraphDbConnection graph)
        {
            this.graph = graph.Client;
        }

        // Question
        public async Task<List<Question>> AddQuestionsAsync(List<Question> questions){
            return null;
        }
        public async Task<List<Question>> GetQuestionsAsync(){
            return new List<Question>(
                       await graph.Cypher
                   .Match("(q:Question)")
                   .ReturnDistinct(q => q.As<Question>())
                   .ResultsAsync
            );
        }
        public async Task<List<Question>> GetQuestionsByConceptOfATechAsync(string Question, string concept){
            return null;
        }
        public async Task<Question> UpdateQuestionAsync(Question question){
            return null;
        }
        public async Task<bool> DeleteQuestionByIdAsync(string QuestionId){
            var result = new List<Question>(await graph.Cypher
                .Match("(q:Question {QuestionId:{id}})")
                .WithParams(new
                {
                    id = QuestionId
                })
                .Return(q => q.As<Question>())
                .ResultsAsync);
            if (result.Count == 0)
            {
                return false;
            }
            await graph.Cypher
                .OptionalMatch("(q:Question)-[relation]->()")
                .Where((Question q) => q.QuestionId == QuestionId)
                .Delete("q, relation")
                .ExecuteWithoutResultsAsync();
            return true;
        }
    }
}