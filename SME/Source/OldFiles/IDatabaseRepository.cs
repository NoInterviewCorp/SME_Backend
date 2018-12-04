// using SME.Models;
// using System.Linq;
// using System.Collections.Generic;
// namespace SME.Persistence
// {
//     public interface IDatabaseRepository
//     {
//         // Concept
//         Concept AddConcept(Concept concept, bool skipFind = false);
//         List<Concept> GetConcepts();
//         Concept GetConceptByName(string name);
//         Concept UpdateConcept(Concept concept);

//         // Resource
//         Resource AddResource(Resource  resource);
//         List<Resource> GetResources();
//         Resource GetResourceByLink(string link);
//         List<Resource> GetResourceByTechnology(string technology);
//         Resource UpdateResource(Resource resource);

//         // Learning Plan
//         LearningPlan AddLearningPlan(LearningPlan learningPlan);
//         IQueryable<LearningPlan> GetLearningPlans();
//         LearningPlan GetLearningPlanById(string learningPlanId);
//         IQueryable<LearningPlan> GetLearningPlansByUserName(string userName);
//         List<LearningPlan> GetLearningPlansByTechnology(string technology);
//         LearningPlan UpdateLearningPlan(LearningPlan learningPlan);

//         // Technology
//         List<Technology> GetAllTechnologies();
//         Technology AddTechnology(Technology technology);
//         Technology GetTechnologyByName(string name);
//         Technology UpdateTechnology(Technology technology);

//         // Question
//         Question AddQuestion(Question question);
//         List<Question> GetQuestions();
//         List<Question> GetQuestionsByConceptOfATech(string technology, string concept);
//         Question UpdateQuestion(Question question);
//     }
// }