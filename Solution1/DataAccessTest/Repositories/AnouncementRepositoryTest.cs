using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessTest.Repositories
{
    [TestClass]
    public class AnouncementRepositoryTest
    {
        private Anouncement CreateAnouncement(Guid courseId, string text = "test", int dateInSecounds = 1000)
        {
            return new Anouncement()
            {
                CourseId = courseId,
                Date = new DateTime(dateInSecounds),
                Text = text
            };
        }

        private DbSet<Anouncement> mockDbSet(IQueryable<Anouncement> anouncements)
        {
            var mockSet = new Mock<DbSet<Anouncement>>();
            mockSet.As<IQueryable<Anouncement>>().Setup(m => m.Provider).Returns(anouncements.Provider);
            mockSet.As<IQueryable<Anouncement>>().Setup(m => m.Expression).Returns(anouncements.Expression);
            mockSet.As<IQueryable<Anouncement>>().Setup(m => m.ElementType).Returns(anouncements.ElementType);
            mockSet.As<IQueryable<Anouncement>>().Setup(m => m.GetEnumerator()).Returns(anouncements.GetEnumerator());
            return mockSet.Object;
        }

        private IPlatformManagement CreateSUT()
        {
            IQueryable<Anouncement> anouncements = new List<Anouncement>()
            {
                CreateAnouncement(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd")),
                CreateAnouncement(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd"), "test2"),
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de")),
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de"), "test3", 122),
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de"), "test12", 1222222)
            }.AsQueryable();

            Mock<IPlatformManagement> platformManagement = new Mock<IPlatformManagement>();
            platformManagement.Setup(m => m.Anouncements).Returns(mockDbSet(anouncements));

            return platformManagement.Object;
        }

        [TestMethod]
        public void When_GetAllyCourseIDIsCalled_Then_AListWithAllTheAnouncesForThatCourseIsReturned()
        {
            IPlatformManagement platformManagement = CreateSUT();
            IAnouncementRepository anouncementRepository = new AnouncementRepository(platformManagement);
            IList<Anouncement> expectedAnouncements = new List<Anouncement>()
            {
                CreateAnouncement(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd")),
                CreateAnouncement(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd"), "test2")
            };
            anouncementRepository.GeAllByCourseId(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd")).Should().Equals(expectedAnouncements);
        }

        [TestMethod]
        public void When_GetAllForPeriodIsCalled_Then_AListWithAllTheAnouncesIsReturnedForThatPeriod()
        {
            IPlatformManagement platformManagement = CreateSUT();
            IAnouncementRepository anouncementRepository = new AnouncementRepository(platformManagement);
            IList<Anouncement> expectedAnouncements = new List<Anouncement>()
            {
                CreateAnouncement(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd")),
                CreateAnouncement(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd"), "test2"),
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de")),
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de"), "test3", 122),
            };
            anouncementRepository.GetAllForPeriod(new DateTime(100), new DateTime(1001)).Should().Equals(expectedAnouncements);
        }

        [TestMethod]
        public void When_GetAllFilteredByPeridoAndCourseIdIsCalled_Then_AListWithAllTheAnouncesIsReturned()
        {
            IPlatformManagement platformManagement = CreateSUT();
            IAnouncementRepository anouncementRepository = new AnouncementRepository(platformManagement);
            IList<Anouncement> expectedAnouncements = new List<Anouncement>()
            {
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de")),
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de"), "test12", 1222222)
            };
            anouncementRepository.GetAllFilteredByPeridoAndCourseId(new DateTime(999), new DateTime(1222244), Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de")).Should().Equals(expectedAnouncements);
        }
    }
}
