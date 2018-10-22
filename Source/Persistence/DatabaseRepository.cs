using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SME.Models;
using OfficeOpenXml;

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

        public List<Technology> GetAllData()
        {
            return context.Technologies.Include(t => t.Topics).ThenInclude(t => t.Questions).ThenInclude(q => q.Options).ToList();
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
        public List<Question> GetAllQuestionsFromTopic(string technology, string topic, bool hasPublished)
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
                                        .Where(q => q.HasPublished == hasPublished && q.TopicId == topicId)
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
                context.Questions.Add(question);
                context.SaveChanges();
                return question;
            }
            return null;
        }

        // used in PUT, updates the technology or the topics inside it
        public Technology UpdateTechnology(Technology technology)
        {
            if (technology.TechnologyId == 0)
            {
                return null;
            }
            Technology technologyObj = context.Technologies
                                        .FirstOrDefault(t => t.TechnologyId == technology.TechnologyId);
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
        public Question UpdateQuestion(Question question)
        {
            Question questionObj = context.Questions.FirstOrDefault(q => q.QuestionId == question.QuestionId);
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
            Question question = context.Questions.FirstOrDefault(q => q.QuestionId == questionId);
            if (question != null)
            {
                context.Questions.Remove(question);
                context.SaveChanges();
                return true;
            }
            return false;
        }
        // deletes a particular technology using it's technology name
        public bool DeleteTechnology(string technology)
        {
            Technology tech = context.Technologies.FirstOrDefault(t => t.Name == technology);
            if (tech != null)
            {
                context.Technologies.Remove(tech);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Question> AddQuestionsFromExcel()
        {
            string pathToExcelFile = @"C:\Users\CGI\Desktop\NoInterviewCorp\SME_Backend\wwwroot\Questions.xlsx";
            List<Question> questions = new List<Question>();
            var workbookFileInfo = new FileInfo(pathToExcelFile);
            using (ExcelPackage excelPackage = new ExcelPackage(workbookFileInfo))
            {
                var totalWorkSheets = excelPackage.Workbook.Worksheets.Count;
                for (int sheetIndex = 1; sheetIndex <= totalWorkSheets; sheetIndex++)
                {
                    var workSheet = excelPackage.Workbook.Worksheets[sheetIndex];
                    Console.WriteLine("Worksheet Name: {0}", workSheet.Name);
                    int rowCount = workSheet.Dimension.Rows;
                    int columnCount = workSheet.Dimension.Columns;
                    for (int rowIndex = 2; rowIndex <= rowCount; rowIndex++)
                    {
                        Question question = new Question();
                        question.ProblemStatement = workSheet.Cells[rowIndex, 2].Value.ToString();
                        question.Options = new List<Option>();
                        for (int columnIndex = 3; columnIndex < 7; columnIndex++)
                        {
                            Option option = new Option
                            {
                                Content = workSheet.Cells[rowIndex, columnIndex].Value.ToString()
                            };
                            question.Options.Add(option);
                        }
                        string[] correctOptions = workSheet.Cells[rowIndex, 7].Value.ToString()
                                                    .Split(new char[] { ' ', ',', '&' });
                        foreach(string correctOption in correctOptions)
                        {
                            int option = 0;
                            if (Int32.TryParse(correctOption, out option))
                            {
                                question.Options[option - 1].IsCorrect = true;
                            }
                            else{
                                throw new ArgumentException("Correct Option Column in the Excel document is filled in an invalid format");
                            }
                        }
                        question.ResourceLink = workSheet.Cells[rowIndex, 8].Value.ToString();
                        int bloomAsInt = 0;
                        if(Int32.TryParse(workSheet.Cells[rowIndex, 9].Value.ToString(), out bloomAsInt)){
                            question.BloomLevel = (BloomTaxonomy)bloomAsInt;
                        }
                        else{
                            throw new ArgumentException("Bloom level is specified in an invalid format in the excel file");
                        }
                        question.HasPublished = true;
                        Topic topic = context.Topics.FirstOrDefault(t => t.Name == workSheet.Cells[rowIndex, 1].Value.ToString());
                        if(topic == null){
                            throw new ArgumentException("Topic name mentioned in the excel file doesn't match with entries inside the database. Please re-enter the topic name");
                        }
                        else{
                            question.TopicId = topic.TopicId;
                        }
                        questions.Add(question);
                        context.Questions.Add(question);
                        context.SaveChanges();
                    }
                }
            }
            return questions;
        }

        public List<Question> UpdateQuestionsFromExcel(string pathToExcelFile)
        {
            List<Question> questions = new List<Question>();
            return questions;
        }
    }
}