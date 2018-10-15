using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SME.Models;

namespace SME.Persistence
{
    public class DatabaseRepository : IDatabaseRepository
    {
        SMEContext context;
        public DatabaseRepository(SMEContext context)
        {
            this.context = context;
        }

        public List<Technology> GetAllTechnologies()
        {
            return context.Technologies.Include(t => t.Topics).ToList();
        }

        public List<Topic> GetAllTopicsInATechnology(string technology)
        {
            var technologies = GetAllTechnologies();
            if (technologies == null)
            {
                return null;
            }
            var tech = technologies.FirstOrDefault(t => t.Name == technology);
            if (tech == null)
            {
                return null;
            }
            List<Topic> topics = tech.Topics;
            return topics;
        }

        public List<Question> GetAllQuestionsFromTopic(string technology, string topic, BloomTaxonomy bloomlevel)
        {
            var topics = GetAllTopicsInATechnology(technology);
            if (topics == null)
            {
                return null;
            }
            var topicObj = topics.FirstOrDefault(t => t.Name == topic);
            if (topicObj == null)
            {
                return null;
            }
            int topicId = topicObj.TopicId;
            List<Question> questions = context.Questions
                                        .Include(q => q.Options)
                                        .Where(q => q.TopicId == topicId)
                                        .ToList();
            return questions;
        }

        public Technology PostToTechnology(Technology technology)
        {
            if (context.Technologies.FirstOrDefault(t => t.Name == technology.Name) == null)
            {
                context.Technologies.Add(technology);
                context.SaveChanges();
                return technology;
            }
            return null;
        }

        public Question PostToTopic(Question question)
        {
            if (context.Questions.FirstOrDefault(q => q.ProblemStatement == question.ProblemStatement) == null)
            {
                context.Questions.Add(question);
                context.SaveChanges();
                return question;
            }
            return null;
        }

        public Technology UpdateTechnology(Technology technology){
            if(technology.TechnologyId == 0){
                return null;
            }
            Technology technologyObj = context.Technologies
                                        .FirstOrDefault(t => t.TechnologyId== technology.TechnologyId);
            if (technologyObj != null)
            {
                context.DetachAllEntities();
                context.Technologies.Update(technology);
                context.SaveChanges();
                return technology;
            }
            return null;
        }

        public Question UpdateQuestions(Question question)
        {
            Question questionObj = context.Questions.FirstOrDefault(q=>q.QuestionId == question.QuestionId);
            if (questionObj != null)
            {
                context.DetachAllEntities();
                context.Questions.Update(question);
                context.SaveChanges();
                return question;
            }
            return null;
        }

        public bool DeleteQuestionById(int questionId)
        {
            // TODO: Add DELETE logic here
            Question question = context.Questions.FirstOrDefault(q=>q.QuestionId == questionId);
            if(question != null){
                context.Questions.Remove(question);
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool DeleteTechnology(string technology){
            Technology tech = context.Technologies.FirstOrDefault(t=>t.Name==technology);
            if(tech!=null){
                context.Technologies.Remove(tech);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}