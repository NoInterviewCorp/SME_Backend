using SME.Models;
using System.Linq;
using System.Collections.Generic;
namespace SME.Persistence
{
    public interface IDatabaseRepository
    {

        List<Resource> GetResources();
        Resource AddResource(Resource  resource);
        Concept AddConcept(Concept concept);
        List<Concept> GetConcepts();
        LearningPlan AddLearningPlan(LearningPlan learningPlan);
        IQueryable<LearningPlan> GetLearningPlans();
        LearningPlan GetLearningPlanById(string learningPlanId);
        IQueryable<LearningPlan> GetLearningPlansByUserName(string userName);
        // List<Technology> GetAllTechnologies();
        // List<Topic> GetAllTopicsInAResource(string Resource);
        // List<Question> GetAllQuestionsFromTopic(string technology, string topic, bool hasPublished);
        // Technology PostToTechnology(Technology technology);
        // Question PostToTopic(Question question);
        // Technology UpdateTechnology(Technology technology);
        // Question UpdateQuestion(Question question);
        // bool DeleteTechnology(string technology);
        // bool DeleteQuestionById(int questionId);
        // List<Question> UpdateQuestionsFromExcel(string pathToExcelFile);
        // List<Question> AddQuestionsFromExcel();

    }
}