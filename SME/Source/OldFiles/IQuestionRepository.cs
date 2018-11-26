using SME.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SME.Persistence
{
    public interface IQuestionRepository
    {
        // Question
        Task<List<Question>> AddQuestionsAsync(List<Question> question);
        Task<List<Question>> GetQuestionsAsync();
        Task<List<Question>> GetQuestionsByConceptOfATechAsync(string technology, string concept);
        Task<Question> UpdateQuestionAsync(Question question);
        Task<bool> DeleteQuestionByIdAsync(string questionId);
    }
}