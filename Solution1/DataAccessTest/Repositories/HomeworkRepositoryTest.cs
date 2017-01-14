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
using Moq;
using Microsoft.EntityFrameworkCore;

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
                Timestamp = new DateTime(10000),
                Observations = "",
                OwesMeMoney = false
            };   
        }

        private DbSet<Homework> mockDbSet()
        {
            var data = new List<Homework>
            {
                InitHomework(Guid.Parse("e6d70219-b169-4d74-8b30-4242b218a744"),
                             Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd"),
                             Guid.Parse("f3610178-09ca-497d-927f-3edff8ab2ea0")),
                InitHomework(Guid.Empty,Guid.Empty,Guid.Empty)
            }.AsQueryable();
            var mockSet = new Mock<DbSet<Homework>>();
            mockSet.As<IQueryable<Homework>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Homework>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Homework>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Homework>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<Homework>())).Callback<Homework>((s) => data.Append(s));
            return mockSet.Object;
        }

        private IPlatformManagement CreateSUT() 
        {
            var mockPlatformManagement = new Mock<IPlatformManagement>();
            mockPlatformManagement.Setup(m => m.Homeworks).Returns(mockDbSet());
            mockPlatformManagement.Setup(m => m.SaveChanges()).Returns(0);
            return mockPlatformManagement.Object;
        } 

        [TestMethod]
        public void When_GetHomeworksByUserIdIsCalled_Then_ShouldReturnExpectedHomework()
        {
            var mockPlatformManagement = CreateSUT();
            _homeworkRepository = new HomeworkRepository(mockPlatformManagement);
            var expected_homeworks = mockPlatformManagement.Homeworks.Where(m => m.UserId == Guid.Parse("f3610178-09ca-497d-927f-3edff8ab2ea0")).ToArray();
            List<Homework> homeworks = _homeworkRepository.
                        GetHomeworksByUserId(Guid.Parse("f3610178-09ca-497d-927f-3edff8ab2ea0"))
                        .ToList();
            
            homeworks.Should().BeEquivalentTo(expected_homeworks);
        }

        [TestMethod]
        public void WhenCreateIsCalled_Then_HomeworkShouldHaveIdSet()
        {
            var mockPlatformManagement = CreateSUT();
            _homeworkRepository = new HomeworkRepository(mockPlatformManagement);
            Homework hw = _homeworkRepository.Create(InitHomework(Guid.Empty, Guid.Empty, Guid.Empty));
            hw.Id.Should().NotBe(Guid.Empty);
        }

        [TestMethod]
        public void WhenCreateIsCalledWithNullArgument_Then_NullArgumentExceptionIsThrown()
        {
            var mockPlatformManagement = CreateSUT();
            _homeworkRepository = new HomeworkRepository(mockPlatformManagement);
            try
            {
                _homeworkRepository.Create(null);
                Assert.Fail();
            }
            catch
            {
            }
        }
    }
}