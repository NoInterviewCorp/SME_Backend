using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SME.Models;

namespace SME.Persistence
{
    public class DatabaseRepository : IDatabaseRepository
    {
        // dependency injection for context class
        SMEContext context;
        public DatabaseRepository(SMEContext context)
        {
            this.context = context;
        }

        // gets all technologies in the database with their respective
        // topics but doesn't include questions
        public List<Technology> GetAllTechnologies()
        {
            return context.Technologies.Include(t => t.Topics).ToList();
        }

        // returns all the topics inside a particular technology without
        // questions
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

        // returns all questions from a particular topic inside a technology
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
                                        .Where(q => q.HasPublished && q.TopicId == topicId)
                                        .ToList();
            return questions;
        }

        // posts a new technology with its respective topics
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

        // posts  a new question inside a topic belonging to a technology
        public Question PostToTopic(Question question)
        {
            if (context.Questions.FirstOrDefault(q => q.ProblemStatement == question.ProblemStatement) == null)
            {
                question.HasPublished = true;
                context.Questions.Add(question);
                context.SaveChanges();
                return question;
            }
            return null;
        }

        // used in PUT, updates the technology or the topics inside it
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

        // used in PUT, updates the questions residing inside a topics
        public Question UpdateQuestions(Question question)
        {
            Question questionObj = context.Questions.FirstOrDefault(q=>q.QuestionId == question.QuestionId);
            if (questionObj != null)
            {
                // detach all entities to prevent error which says
                // cannot update a tracked column
                context.DetachAllEntities();
                context.Questions.Update(question);
                context.SaveChanges();
                return question;
            }
            return null;
        }

        // deletes a question using it's auto generated id which can be 
        // found when aa get call for questions is done
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
        // deletes a particular technology using it's technology name
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