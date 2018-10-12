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
            List<Topic> topics = new List<Topic>();
            return topics;
        }

        public List<Question> GetAllQuestionsFromTopic(string technology, string topic, BloomTaxonomy bloomlevel){
            List<Question> questions = new List<Question>();
            return questions;
        }

        public Technology PostToTechnology(Technology technology){
            return new Technology();;
        }

        public Technology UpdateQuestions(string techName, Technology technology){
            return new Technology();
        }

        public bool DeleteQuestionById(string technology, string topic, int questionId){
            return false;
        }
    }
}