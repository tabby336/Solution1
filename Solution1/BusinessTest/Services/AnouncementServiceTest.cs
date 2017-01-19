using Business.Services;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace BusinessTest.Services
{
    [TestClass]
    public class AnouncementServiceTest
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

        [TestMethod]
        public void When_GetAllIsCalled_Then_AllAnouncesAreReturned()
        {
            List<Anouncement> anouncements = new List<Anouncement>()
            {
                CreateAnouncement(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd")),
                CreateAnouncement(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd"), "test2"),
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de")),
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de"), "test3", 122),
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de"), "test12", 1222222)
            };
            Mock<IAnouncementRepository> anouncementRepository = new Mock<IAnouncementRepository>();
            anouncementRepository.Setup(m => m.GetAll()).Returns(anouncements);

            AnouncementService anouncementService = new AnouncementService(anouncementRepository.Object);

            anouncementService.GetAllAnouncements().Should().Equal(anouncements);
        }

        [TestMethod]
        public void When_GetAllAnouncements_Then_AllAnouncesAreReturned()
        {
            List<Anouncement> anouncements = new List<Anouncement>()
            {
                CreateAnouncement(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd")),
                CreateAnouncement(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd"), "test2")
            };
            Mock<IAnouncementRepository> anouncementRepository = new Mock<IAnouncementRepository>();
            anouncementRepository.Setup(m => m.GeAllByCourseId(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd"))).Returns(anouncements);

            AnouncementService anouncementService = new AnouncementService(anouncementRepository.Object);

            anouncementService.GetAllByCourse(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd")).Should().Equal(anouncements);
        }

        [TestMethod]
        public void When_GetAllByPeriod_Then_AllAnouncesAreReturned()
        {
            List<Anouncement> anouncements = new List<Anouncement>()
            {
                CreateAnouncement(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd")),
                CreateAnouncement(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054dd"), "test2"),
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de")),
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de"), "test3", 122),
            };
            Mock<IAnouncementRepository> anouncementRepository = new Mock<IAnouncementRepository>();
            anouncementRepository.Setup(m => m.GetAllForPeriod(new DateTime(100), new DateTime(1001))).Returns(anouncements);

            AnouncementService anouncementService = new AnouncementService(anouncementRepository.Object);

            anouncementService.GetAllByPeriod(new DateTime(100), new DateTime(1001)).Should().Equal(anouncements);
        }

        [TestMethod]
        public void When_GetAllFilteredByCourseAndPeriod_Then_AllAnouncesAreReturned()
        {
            List<Anouncement> anouncements = new List<Anouncement>()
            {
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de")),
                CreateAnouncement(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de"), "test3", 122)
            };
            Mock<IAnouncementRepository> anouncementRepository = new Mock<IAnouncementRepository>();
            anouncementRepository.Setup(m => m.GetAllFilteredByPeridoAndCourseId(new DateTime(100), (new DateTime(1001)).Date, Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de"))).Returns(anouncements);

            AnouncementService anouncementService = new AnouncementService(anouncementRepository.Object);

            var x = anouncementService.GetAllFilteredByCourseAndPeriod(new DateTime(100), new DateTime(1001), Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054de"));
            x.Should().Equal(anouncements);
        }
    }
}
