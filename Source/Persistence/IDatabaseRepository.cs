using SME.Models;
using System.Collections.Generic;
namespace SME.Persistence
{
    public interface IDatabaseRepository
    {
        List<Technology> GetAllTechnologies();
        List<Topic> GetAllTopicsInATechnology(string technology);
        List<Question> GetAllQuestionsFromTopic(string technology, string topic, BloomTaxonomy bloomlevel);
        Technology PostToTechnology(Technology technology);
        Question PostToTopic(Question question);
        Technology UpdateTechnology(Technology technology);
        Question UpdateQuestions(Question question);
        bool DeleteTechnology(string technology);
        bool DeleteQuestionById(int questionId);

    }
}