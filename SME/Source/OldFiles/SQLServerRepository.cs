// using System;
// using System.IO;
// using System.Linq;
// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore;
// using SME.Models;

// namespace SME.Persistence
// {
//     public class SQLServerRepository : IDatabaseRepository
//     {
//         // dependency injection for context class
//         private SMEContext context;
//         public SQLServerRepository(SMEContext context)
//         {
//             this.context = context;
//         }

//         // Concept Controller Functions

//         public Concept AddConcept(Concept concept, bool skipFind = false)
//         {
//             if (skipFind || context.Concepts.FirstOrDefault(c => c.Name == concept.Name) == null)
//             {
//                 concept.ConceptId = Guid.NewGuid().ToString("N");
//                 context.Concepts.Add(concept);
//                 if (!skipFind) { context.SaveChanges(); }
//                 return concept;
//             }
//             return null;
//         }

//         public List<Concept> GetConcepts()
//         {
//             return context.Concepts.ToList();
//         }

//         public Concept GetConceptByName(string name)
//         {
//             return context.Concepts.FirstOrDefault(c => c.Name == name);
//         }

//         public Concept UpdateConcept(Concept concept)
//         {
//             if (context.Concepts.FirstOrDefault(c => c.ConceptId == concept.ConceptId) != null)
//             {

//                 context.DetachAllEntities();
//                 context.Concepts.Update(concept);
//                 context.SaveChanges();
//                 return concept;
//             }
//             return null;
//         }


//         // Resource Controller

//         public Resource AddResource(Resource resource)
//         {
//             if (context.Resources.FirstOrDefault(r => r.ResourceLink == resource.ResourceLink) == null)
//             {
//                 resource.ResourceId = Guid.NewGuid().ToString("N");
//                 context.Resources.Add(resource);
//                 context.SaveChanges();
//                 return resource;
//             }
//             return null;
//         }

//         public List<Resource> GetResources()
//         {
//             return context.Resources.Include(r => r.ResourceConcepts).ToList();
//         }

//         public Resource GetResourceByLink(string link)
//         {
//             return context.Resources.FirstOrDefault(r => r.ResourceLink == link);
//         }

//         public List<Resource> GetResourceByTechnology(string technology)
//         {
//             var techObj = context.Technologies.FirstOrDefault(t => t.Name == technology);
//             if (techObj != null)
//             {
//                 List<Resource> resources = new List<Resource>();
//                 var listRT = context.ResourceTechnologies.Include(rt => rt.Resource).Where(rt => rt.TechnologyId == techObj.TechnologyId).ToList();
//                 foreach (ResourceTechnology rt in listRT)
//                 {
//                     resources.Add(rt.Resource);
//                 }
//                 return resources;
//             }
//             return null;
//         }

//         public Resource UpdateResource(Resource resource)
//         {
//             if (context.Resources.FirstOrDefault(r => r.ResourceId == resource.ResourceId) != null)
//             {
//                 context.DetachAllEntities();
//                 context.Resources.Update(resource);
//                 context.SaveChanges();
//                 return resource;
//             }
//             return null;
//         }

//         // LearningPlan Controller Functions

//         public LearningPlan AddLearningPlan(LearningPlan learningPlan)
//         {
//             if (context.LearningPlan.FirstOrDefault(lp => (lp.UserName == learningPlan.UserName) && (lp.Name == learningPlan.Name)) == null)
//             {
//                 learningPlan.LearningPlanId = Guid.NewGuid().ToString("N");
//                 foreach (Topic topic in learningPlan.Topics)
//                 {
//                     topic.TopicId = Guid.NewGuid().ToString("N");
//                 }
//                 context.LearningPlan.Add(learningPlan);
//                 context.SaveChanges();
//                 return learningPlan;
//             }
//             return null;
//         }

//         public IQueryable<LearningPlan> GetLearningPlans()
//         {
//             return context.LearningPlan.Include(lp => lp.Topics);
//         }

//         public IQueryable<LearningPlan> GetLearningPlansByUserName(string userName)
//         {
//             return context.LearningPlan.Where(lp => lp.UserName == userName);
//         }

//         public LearningPlan GetLearningPlanById(string learningPlanId)
//         {
//             return context.LearningPlan.FirstOrDefault(lp => lp.LearningPlanId == learningPlanId);
//         }

//         public List<LearningPlan> GetLearningPlansByTechnology(string technology)
//         {
//             return context.LearningPlan.Where(lp => lp.Technology.Name == technology).ToList();
//         }

//         public LearningPlan UpdateLearningPlan(LearningPlan learningPlan)
//         {
//             if (context.LearningPlan.FirstOrDefault(lp => lp.LearningPlanId == learningPlan.LearningPlanId) != null)
//             {
//                 context.DetachAllEntities();
//                 context.LearningPlan.Update(learningPlan);
//                 context.SaveChanges();
//                 return learningPlan;
//             }
//             return null;
//         }

//         // Technology Controller Function

//         public List<Technology> GetAllTechnologies()
//         {
//             return context.Technologies.ToList();
//         }

//         public Technology GetTechnologyByName(string name)
//         {
//             return context.Technologies.FirstOrDefault(t => t.Name == name);
//         }

//         public Technology AddTechnology(Technology technology)
//         {
//             if (context.Technologies.FirstOrDefault(t => t.Name == technology.Name) == null)
//             {
//                 technology.TechnologyId = Guid.NewGuid().ToString("N");
//                 context.Technologies.Add(technology);
//                 context.SaveChanges();
//                 return technology;
//             }
//             return null;
//         }

//         public Technology UpdateTechnology(Technology technology)
//         {
//             if (context.Technologies.FirstOrDefault(t => t.TechnologyId == technology.TechnologyId) != null)
//             {
//                 context.DetachAllEntities();
//                 context.Technologies.Update(technology);
//                 context.SaveChanges();
//                 return technology;
//             }
//             return null;
//         }

//         public Question AddQuestion(Question question)
//         {
//             if (context.Questions.FirstOrDefault(q => q.ProblemStatement == question.ProblemStatement) == null)
//             {
//                 question.QuestionId = Guid.NewGuid().ToString("N");
//                 context.Questions.Add(question);
//                 context.SaveChanges();
//                 return question;
//             }
//             return null;
//         }

//         public List<Question> GetQuestions()
//         {
//             return context.Questions.ToList();
//         }

//         public List<Question> GetQuestionsByConceptOfATech(string technology, string concept)
//         {
//             string technologyId = context.Technologies.FirstOrDefault(t => t.Name.ToLower() == technology.ToLower()).TechnologyId;
//             string conceptId = context.Concepts.FirstOrDefault(c => c.Name.ToLower() == concept.ToLower()).ConceptId;
//             return context.Questions.Include(q => q.ConceptQuestions)
//                 .Where(q => 
//                     (q.TechnologyId == technologyId)
//                     &&
//                     (q.ConceptQuestions.Where(cq => cq.ConceptId == conceptId) != null)
//                 ).ToList();

//         }

//         public Question UpdateQuestion(Question question)
//         {
//             if (context.Questions.FirstOrDefault(q => q.QuestionId == question.QuestionId) != null)
//             {
//                 context.DetachAllEntities();
//                 context.Questions.Update(question);
//                 context.SaveChanges();
//                 return question;
//             }
//             return null;
//         }

//     }
// }