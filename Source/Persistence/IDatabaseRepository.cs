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
        Technology UpdateQuestions(string topicName, Technology technology);
        bool DeleteQuestionById(string technology, string topic, int questionId );
        
    }
}