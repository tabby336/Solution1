using System;
using System.Collections.Generic;
using Business.Services;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessTest.Services
{ 
    [TestClass]
    public class MarkServiceTest
    {
        private static List<Mark> CreateSut()
        {
            var firstElement = new Mark
            {
                HomeworkId = Guid.Parse("3a6021f9-864c-429f-9878-fe382ec2e56a"),
                UserId = Guid.Parse("89e4dcb2-3f25-4f7c-bb3f-b2a96ae4b6e6"),
                Value = 10,
                Description = "",
                Timestamp = new DateTime(2008, 3, 1, 7, 0, 0),
                CreatorId = Guid.Parse("86f8f04e-88cb-4754-a487-1ff54250107a"),
                HasComment = false,
                HasContestation = false
            };
            var secondElement = new Mark
            {
                HomeworkId = Guid.Parse("3a6021f9-864c-429f-9878-fe382ec2e56f"),
                UserId = Guid.Parse("89e4dcb2-3f25-4f7c-bb3f-b2a96ae4b6e6"),
                Value = 6,
                Description = "",
                Timestamp = new DateTime(2008, 3, 1, 7, 0, 0),
                CreatorId = Guid.Parse("86f8f04e-88cb-4754-a487-1ff54250107d"),
                HasComment = false,
                HasContestation = false
            };
            var thirdElement = new Mark
            {
                HomeworkId = Guid.Parse("3a6021f9-864c-429f-9878-fe382ec2e56f"),
                UserId = Guid.Parse("89e4dcb2-3f25-1111-bb3f-b2a96ae4b6e6"),
                Value = 6,
                Description = "",
                Timestamp = new DateTime(2008, 3, 1, 7, 0, 0),
                CreatorId = Guid.Parse("86f8f04e-88cb-4754-a487-1ff54250107d"),
                HasComment = false,
                HasContestation = false
            };
            return new List<Mark> { firstElement, secondElement, thirdElement };
        }

        [TestMethod]
        public void When_FilterMarksByUserIsCalledWithAnUserIdThatHasAMark_Then_ReturnItsMarks() {
            var marks = CreateSut();
            var mockDataAccess = new Mock<IMarkRepository>();
            var mockCourseRepository = new Mock<ICourseRepository>();
            var mockModuleRepository = new Mock<IModuleRepository>();
            mockDataAccess.Setup(m => m.GetMarksByUserId(Guid.Parse("89e4dcb2-3f25-4f7c-bb3f-b2a96ae4b6e6"))).Returns(marks);
            var productBusiness = new MarkService(mockDataAccess.Object, mockCourseRepository.Object, mockModuleRepository.Object);
            productBusiness.FilterMarksByUser("89e4dcb2-3f25-4f7c-bb3f-b2a96ae4b6e6").Should().BeEquivalentTo(marks);
        }

        [TestMethod]
        public void When_FilterMarksByUserIsCalledWithAnUserIdThatDoesNotHasAMark_Then_ReturnNull()
        {
            var mockDataAccess = new Mock<IMarkRepository>();
            var mockCourseRepository = new Mock<ICourseRepository>();
            var mockModuleRepository = new Mock<IModuleRepository>();
            mockDataAccess.Setup(m => m.GetMarksByUserId(Guid.Parse("89e4dcb2-3f25-4f7c-bb3f-b2a96ae4b6e6"))).Returns((List<Mark>)null);
            var productBusiness = new MarkService(mockDataAccess.Object, mockCourseRepository.Object, mockModuleRepository.Object);
            productBusiness.FilterMarksByUser("89e4dcb2-3f25-4f7c-bb3f-b2a96ae4b6e6").Should().Equal(null);
        }

        [TestMethod]
        public void When_FilterMarksByUserIsCalledWithANullId_Then_ReturnNull()
        {
            var mockDataAccess = new Mock<IMarkRepository>();
            var mockCourseRepository = new Mock<ICourseRepository>();
            var mockModuleRepository = new Mock<IModuleRepository>();
            var productBusiness = new MarkService(mockDataAccess.Object, mockCourseRepository.Object, mockModuleRepository.Object);
            productBusiness.FilterMarksByUser(null).Should().Equal(null);
        }

        [TestMethod]
        public void When_FilterMarksByUserIsCalledWithAnInvalidId_Then_ReturnNull()
        {
            var mockDataAccess = new Mock<IMarkRepository>();
            var mockCourseRepository = new Mock<ICourseRepository>();
            var mockModuleRepository = new Mock<IModuleRepository>();
            var productBusiness = new MarkService(mockDataAccess.Object, mockCourseRepository.Object, mockModuleRepository.Object);
            productBusiness.FilterMarksByUser("test").Should().Equal(null);
        }

    }
}
