using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SME.Models;

namespace SME.Persistence{
    public class DatabaseRepository : IDatabaseRepository{
        SMEContext context;
        public DatabaseRepository(SMEContext context)
        {
            this.context = context;
        }

        public List<Technology> GetAllTechnologies(){
            return context.Technologies.Include(t=>t.Topics).ToList();
        }

        public List<Topic> GetAllTopicsInATechnology(string technology){
            var technologies = GetAllTechnologies();
            if(technologies == null){
                return null;
            }
            var tech = technologies.FirstOrDefault(t => t.Name == technology);
            if(tech == null){
                return null;
            }
            List<Topic> topics = tech.Topics;
            return topics;
        }

        public List<Question> GetAllQuestionsFromTopic(string technology, string topic, BloomTaxonomy bloomlevel){
            var topics = GetAllTopicsInATechnology(technology); 
            if(topics == null){
                return null;
            }
            var topicObj = topics.FirstOrDefault(t => t.Name == topic);
            if(topicObj == null){
                return null;
            }
            int topicId = topicObj.TopicId;
            List<Question> questions =  context.Questions
                                        .Include(q=>q.Options)
                                        .Where(q=> q.TopicId == topicId)
                                        .ToList();
            return questions;
        }

        public Technology PostToTechnology(Technology technology){
            if(context.Technologies.FirstOrDefault(t=>t.Name == technology.Name) == null){
                context.Technologies.Add(technology);
                context.SaveChanges();
                return technology;
            }
            return null;
        }

        public Technology UpdateQuestions(string topicName, Technology technology){
            
            if(GetAllTopicsInATechnology(technology.Name).FirstOrDefault(t => t.Name == topicName) != null){
                // TODO: Add UPDATE database logic here 
                return technology;
            }
            return null;
        }

        public bool DeleteQuestionById(string technology, string topic, int questionId){
            // TODO: Add DELETE logic here
            return false;
        }
    }
}