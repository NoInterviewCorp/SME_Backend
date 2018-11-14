using SME.Models;
using System.Collections.Generic;
namespace SME.Persistence
{
    public interface IQuestionRepository
    {
        // Question
        Question AddQuestion(Question question);
        List<Question> GetQuestions();
        List<Question> GetQuestionsByConceptOfATech(string technology, string concept);
        Question UpdateQuestion(Question question);
    }
}