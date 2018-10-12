using System;
using System.Collections.Generic;
using SME.Models;

namespace SME.Persistence{
    public class DatabaseRepository : IDatabaseRepository{
        SMEContext context;
        public DatabaseRepository(SMEContext context)
        {
            this.context = context;
        }

        public List<Technology> GetAllTechnologies(){
            List<Technology> technologies = new List<Technology>();
            return technologies;
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

        }

        public boolean DeleteQuestionById(string technology, string topic, int questionId){
            
        }
    }
}