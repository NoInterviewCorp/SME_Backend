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

        public List<Topic> GetAllTopicsInATechnology(){
            List<Topic> topics = new List<Topic>();
            return topics;
        }
    }
}