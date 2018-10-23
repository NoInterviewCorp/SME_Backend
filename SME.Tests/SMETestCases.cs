using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.Controllers;
using SME.Models;
using SME.Persistence;
using Xunit;
namespace SME.Tests
{
    public class SMETestCases
    {

        public List<Technology> GetMockDatabase()
        {
            List<Technology> technologies = new List<Technology>{
                new Technology{
                    TechnologyId=1,
                    Name="C#",
                    Topics = new List<Topic>{
                        new Topic{
                            TopicId=1,
                            Name="Introduction",
                            Questions=new List<Question>{
                                new Question{
                                    QuestionId=1,
                                    ProblemStatement="What is C# used for?",
                                    Options=new List<Option>(),
                                    ResourceLink="yur.web.site",
                                    BloomLevel = BloomTaxonomy.Knowledge,
                                    HasPublished=true,
                                    TopicId=1
                                },
                                new Question{
                                    QuestionId=2,
                                    ProblemStatement="What is C# ?",
                                    Options=new List<Option>(),
                                    ResourceLink="yur.web.site",
                                    BloomLevel = BloomTaxonomy.Comprehension,
                                    HasPublished=true,
                                    TopicId=1
                                }
                            },
                            TechnologyId = 1
                        }
                    }
                }
            };
            return technologies;
        }

        [Fact]
        public void GetAll_Positive_ReturnsList()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            List<Technology> technologies = GetMockDatabase();
            dataRepo.Setup(d => d.GetAllData()).Returns(technologies);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.GetAll();

            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as List<Technology>;
            Assert.NotNull(model);

            Assert.Equal(technologies.Count, model.Count);
        }

        [Fact]
        public void GetAll_Negative_ReturnsEmptyList()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            List<Technology> list = null;
            dataRepo.Setup(d => d.GetAllData()).Returns(list);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.GetAll();

            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as List<Technology>;
            Assert.NotNull(model);
            int expected = 0;
            Assert.Equal(expected, model.Count);
        }

        [Fact]
        public void GetTechnologies_Positive_ReturnsList()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            List<Technology> technologies = GetMockDatabase();
            dataRepo.Setup(d => d.GetAllTechnologies()).Returns(technologies);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Get();

            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as List<Technology>;
            Assert.NotNull(model);

            Assert.Equal(technologies.Count, model.Count);
        }

        [Fact]
        public void GetTechnologies_Negative_ReturnsEmptyList()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            List<Technology> list = null;
            dataRepo.Setup(d => d.GetAllTechnologies()).Returns(list);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Get();

            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as List<Technology>;
            Assert.NotNull(model);
            int expected = 0;
            Assert.Equal(expected, model.Count);
        }

        [Fact]
        public void GetAllTopicsInATechnology_Positive_ReturnsTopicsList()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            string technology = "C#";
            var list = GetMockDatabase().FirstOrDefault(t => t.Name == technology).Topics;
            dataRepo.Setup(d => d.GetAllTopicsInATechnology(technology)).Returns(list);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Get(technology, null, false);

            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as List<Topic>;
            Assert.NotNull(model);
            Assert.Equal(list.Count, model.Count);
        }
        [Fact]
        public void GetAllTopicsInATechnology_Negative_NotFound()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            string technology = "Java";
            List<Topic> list = null;
            dataRepo.Setup(d => d.GetAllTopicsInATechnology(technology)).Returns(list);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Get(technology, null, false);

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public void GetAllQuestionsInATech_Positive_ReturnsQuestionList()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            string technology = "C#";
            string topic = "Introduction";
            bool hasPublished = true;
            var list = GetMockDatabase().FirstOrDefault(t => t.Name == technology)
                        .Topics.FirstOrDefault(t => t.Name == topic)
                        .Questions;
            dataRepo.Setup(d => d.GetAllQuestionsFromTopic(technology, topic, hasPublished)).Returns(list);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Get(technology, topic, hasPublished);

            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as List<Question>;
            Assert.NotNull(model);
            Assert.Equal(list.Count, model.Count);
        }
        [Fact]
        public void GetAllQuestionsInATech_Negative_ReturnsNotFound()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            string technology = "Java";
            string topic = "OOPS";
            bool hasPublished = false;
            List<Question> list = null;
            dataRepo.Setup(d => d.GetAllQuestionsFromTopic(technology, topic, hasPublished)).Returns(list);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Get(technology, topic, hasPublished);

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public void PostTechnology_Positive_ReturnsCreatedObject()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            Technology technology = new Technology
            {
                Name = "Java",
                Topics = new List<Topic>()
            };
            dataRepo.Setup(d => d.PostToTechnology(technology)).Returns(technology);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Post(technology);
            Assert.NotNull(actionResult);

            var createdResult = actionResult as CreatedResult;
            Assert.NotNull(createdResult);

            var model = createdResult.Value as Technology;
            Assert.NotNull(model);
        }

        [Fact]
        public void PostTechnology_Negative_ReturnsBadRequest()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            Technology technology = null;
            dataRepo.Setup(d => d.PostToTechnology(technology)).Returns(technology);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Post(technology);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public void PostQuestion_Positive_ReturnsCreatedObject()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            Question question = new Question();
            dataRepo.Setup(d => d.PostToTopic(question)).Returns(question);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Post(question);
            Assert.NotNull(actionResult);

            var createdResult = actionResult as CreatedResult;
            Assert.NotNull(createdResult);

            var model = createdResult.Value as Question;
            Assert.NotNull(model);
        }

        [Fact]
        public void PostQuestion_Negative_ReturnsBadRequest()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            Question question = null;
            dataRepo.Setup(d => d.PostToTopic(question)).Returns(question);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Post(question);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }


        [Fact]
        public void PutTechnology_Positive_ReturnsUpdatedObject()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            Technology technology = new Technology
            {
                Name = "Java",
                Topics = new List<Topic>()
            };
            dataRepo.Setup(d => d.UpdateTechnology(technology)).Returns(technology);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Put(technology);
            Assert.NotNull(actionResult);

            var createdResult = actionResult as CreatedResult;
            Assert.NotNull(createdResult);

            var model = createdResult.Value as Technology;
            Assert.NotNull(model);
        }

        [Fact]
        public void PutTechnology_Negative_ReturnsBadRequest()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            Technology technology = null;
            dataRepo.Setup(d => d.UpdateTechnology(technology)).Returns(technology);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Put(new Technology());
            Assert.IsType<NotFoundObjectResult>(actionResult);
        }
        [Fact]
        public void DeleteTechnology_Positive_ReturnsOk()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            string technology = "C#";
            dataRepo.Setup(d => d.DeleteTechnology(technology)).Returns(true);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Delete(technology, 0);
            Assert.NotNull(actionResult);

            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult);
        }
        [Fact]
        public void DeleteTechnology_Negative_ReturnsNotFound()
        {
            var dataRepo = new Mock<IDatabaseRepository>();
            string technology = "Java";
            dataRepo.Setup(d => d.DeleteTechnology(technology)).Returns(false);
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.Delete(technology, 0);
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundObjectResult>(actionResult);
        }
    }
}
