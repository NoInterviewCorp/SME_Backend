using SME.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SME.Persistence
{
    public interface IQuestionRepository
    {
        // Question
        Task<List<Question>> AddQuestionsAsync(List<Question> questions, string resourceId);
        Task<List<Question>> GetQuestionsAsync();
        Task<List<Question>> GetQuestionsByResourceAsync(string resourceId);
        Task<Question> UpdateQuestionAsync(Question question);
        Task<bool> DeleteQuestionByIdAsync(string questionId);
    }
}