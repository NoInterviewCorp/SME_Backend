using SME.Models;
using System.Collections.Generic;
namespace SME.Persistence
{
    public interface IDatabaseRepository
    {
        List<Technology> GetAllData();
        List<Technology> GetAllTechnologies();
        List<Topic> GetAllTopicsInATechnology(string technology);
        List<Question> GetAllQuestionsFromTopic(string technology, string topic, bool hasPublished);
        Technology PostToTechnology(Technology technology);
        Question PostToTopic(Question question);
        Technology UpdateTechnology(Technology technology);
        Question UpdateQuestion(Question question);
        bool DeleteTechnology(string technology);
        bool DeleteQuestionById(int questionId);
        List<Question> UpdateQuestionsFromExcel(string pathToExcelFile);
        List<Question> AddQuestionsFromExcel();

    }
}