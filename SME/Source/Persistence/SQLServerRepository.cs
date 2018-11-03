using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SME.Models;

namespace SME.Persistence
{
    public class SQLServerRepository : IDatabaseRepository
    {
        // dependency injection for context class
        private SMEContext context;
        public SQLServerRepository(SMEContext context)
        {
            this.context = context;
        }

        public Resource AddResource(Resource resource)
        {
            if (context.Resources.FirstOrDefault(r => r.ResourceLink == resource.ResourceLink) == null)
            {
                resource.ResourceId = Guid.NewGuid().ToString("N");
                context.Resources.Add(resource);
                context.SaveChanges();
                return resource;
            }
            return null;
        }

        public List<Resource> GetResources()
        {
            return context.Resources.Include(r=>r.ResourceConcepts).ToList();
        }

        public Concept AddConcept(Concept concept)
        {
            if (context.Concepts.FirstOrDefault(c => c.Name == concept.Name) == null)
            {
                concept.ConceptId = Guid.NewGuid().ToString("N");
                context.Concepts.Add(concept);
                context.SaveChanges();
                return concept;
            }
            return null;
        }

        public List<Concept> GetConcepts(){
            return context.Concepts.ToList();
        }

        public LearningPlan AddLearningPlan(LearningPlan learningPlan){
            if(context.LearningPlan.FirstOrDefault(lp=>(lp.UserName==learningPlan.UserName) && (lp.Name==learningPlan.Name))==null){
                learningPlan.LearningPlanId = Guid.NewGuid().ToString("N");
                context.LearningPlan.Add(learningPlan);
                context.SaveChanges();
                return learningPlan;
            }
            return null;
        }

        public IQueryable<LearningPlan> GetLearningPlans(){
            return context.LearningPlan.Include(lp=>lp.Topics);
        }

        public IQueryable<LearningPlan> GetLearningPlansByUserName(string userName){
            return context.LearningPlan.Where(lp=>lp.UserName == userName);
        }

        public LearningPlan GetLearningPlanById(string learningPlanId){
            return context.LearningPlan.FirstOrDefault(lp=>lp.LearningPlanId==learningPlanId);
        }
    }
}