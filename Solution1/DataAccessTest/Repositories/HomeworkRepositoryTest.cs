using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.DotNet.ProjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessTest.Repositories
{
    [TestClass]
    public class HomeworkRepositoryTest
    {
        private IHomeworkRepository _homeworkRepository;
        private IEnumerable<Homework> _homeworks;

        private Homework InitHomework(Guid id, Guid mid, Guid uid)
        {
            return new Homework
            {
                Id = id,
                ModuleId = mid,
                UserId = uid,
                Timestamp = DateTime.Now,
                Observations = "",
                OwesMeMoney = false
            };   
        }

        private IEnumerable<Homework> CreateSUT() 
        {
            return new List<Homework> 
            {
                InitHomework(Guid.Parse("e6d70219-b169-4d74-8b30-4242b218a744"),
                             Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd"),
                             Guid.Parse("f3610178-09ca-497d-927f-3edff8ab2ea0")),
                InitHomework(Guid.Empty,Guid.Empty,Guid.Empty)
            };
        } 

        [TestInitialize]
        public void SetUp()
        {
            PlatformManagement dbContext = new PlatformManagement();
            _homeworkRepository = new HomeworkRepository(dbContext);

            _homeworks = CreateSUT();        
            _homeworkRepository.Create(_homeworks.ToList()[0]);
        }

        [TestCleanup]
        public void TearDown()
        {
            _homeworkRepository.Delete(_homeworks.ToList()[0]);
            _homeworkRepository = null;
        }

        [TestMethod]
        public void When_GetHomeworksByUserIdIsCalled_Then_ShouldReturnExpectedHomework()
        {
            List<Homework> homeworks = _homeworkRepository.
                        GetHomeworksByUserId(Guid.Parse("f3610178-09ca-497d-927f-3edff8ab2ea0"))
                        .ToList();
            homeworks.Count().Should().Be(1);
        }

        [TestMethod]
        public void WhenCreateIsCalled_Then_HomeworkShouldHaveIdSet()
        {

            Homework hw = _homeworkRepository.Create(_homeworks.ToList()[1]);
            hw.Id.Should().NotBe(Guid.Empty);
            _homeworkRepository.Delete(hw);

        }
    }
}