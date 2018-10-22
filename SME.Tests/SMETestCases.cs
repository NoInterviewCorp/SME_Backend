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
                {
                    TechnologyId=1,
                    Name="C#",
                    Topics = new List<Topic>{
                        {
                            TopicId=1,
                            Name="Introduction",
                            Questions=new List<Question>{
                                {
                                    QuestionId=1,
                                    ProblemStatement="What is C# used for?",
                                    Options=new List<Option>(),
                                    ResourceLink="yur.web.site",
                                    BloomLevel = BloomTaxonomy.Knowledge,
                                    TopicId=1
                                },
                                {
                                    QuestionId=2,
                                    ProblemStatement="What is C# ?",
                                    Options=new List<Option>(),
                                    ResourceLink="yur.web.site",
                                    BloomLevel = BloomTaxonomy.Comprehension,
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
        public void GetAll_Positive_ReturnsList(){
            var dataRepo = new Mock<IDatabaseRepository>();
            List<Technology> technologies = GetMockDatabase();
            dataRepo.Setup(d => d.GetAllData().Returns(technologies));
            SMEController sMEController = new SMEController(dataRepo.Object);

            var actionResult = sMEController.GetAll();

            var okObjectResult = actionResult as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as List<Technology>;
            Assert.NotNull(model);

            Assert.Equal(technologies.Count, model.Count);
        }
    }
}
