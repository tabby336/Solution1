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
using Business.CommonInfrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Business.Services;

namespace BusinessTest
{
    [TestClass]
    public class HomeworkServiceTest
    {
        private List<Homework> addedHomework;

        private Homework HomeworkInit(string uid, string mid, string obs)
        {
            return new Homework
            {
                ModuleId = Guid.Parse(mid),
                UserId = Guid.Parse(uid),
                Timestamp = new DateTime(10000),
                Observations = obs
            };
        }

        IUpload CreateUpload(List<string> pathList)
        {
            var mockUpload = new Mock<IUpload>();
            mockUpload.Setup(m => m.UploadFiles(It.IsAny<IList<IFormFile>>(), It.IsAny<string>())).Returns(pathList);
            return mockUpload.Object;
        }

        private Mock<DbSet<Homework>> MockHomeworkDbSet(IQueryable<Homework> data)
        {
            var mockSet = new Mock<DbSet<Homework>>();
            mockSet.As<IQueryable<Homework>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Homework>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Homework>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Homework>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        private IHomeworkRepository mockHomeworkRepositoryExpectedBehaviour()
        {
            var data = new List<Homework>().AsQueryable();
            var mockSet = MockHomeworkDbSet(data);
            Mock<IHomeworkRepository> mockHomeworkRepository = new Mock<IHomeworkRepository>();
            addedHomework = new List<Homework>();
            mockHomeworkRepository.Setup(m => m.Create(It.IsAny<Homework>())).Callback<Homework>(s => addedHomework.Add(s)).Returns(new Homework());
            return mockHomeworkRepository.Object;
        }

        private IHomeworkRepository mockHomeworkDataBaseInsertError()
        {
            var data = new List<Homework>().AsQueryable();
            var mockSet = MockHomeworkDbSet(data);
            Mock<IHomeworkRepository> mockHomeworkRepository = new Mock<IHomeworkRepository>();
            addedHomework = new List<Homework>();
            mockHomeworkRepository.Setup(m => m.Create(It.IsAny<Homework>())).Returns((Homework)null);
            return mockHomeworkRepository.Object;
        }

        [TestMethod]
        public void When_UploadIsCalled_Then_TheFilesAreSavedIntoDatabase()
        {
            var homeworkRepository = mockHomeworkRepositoryExpectedBehaviour();
            HomeworkService homeworkService = new HomeworkService(homeworkRepository);
            List<string> expectedUrls = new List<string>(new string[] { "test1", "test2" });
            string messageToBePrinted = homeworkService.Upload(CreateUpload(expectedUrls), new List<IFormFile>(), "f3610178-09ca-497d-927f-3edff8ab2ea0", "f3610178-09ca-497d-927f-3edff8ab2ea1", "None");
            for (int i = 0; i < addedHomework.Count; ++i)
            {
                Homework homework = addedHomework[i];
                homework.Observations.Should().Be("None");
                homework.ModuleId.Should().Be(Guid.Parse("f3610178-09ca-497d-927f-3edff8ab2ea1"));
                homework.UserId.Should().Be(Guid.Parse("f3610178-09ca-497d-927f-3edff8ab2ea0"));
                homework.Url.Should().Be(expectedUrls[i]);
            }
            messageToBePrinted.Should().Be("Upload successfully!");
        }

        [TestMethod]
        public void When_UploadIsCalledAndAnErrorOccuredWhileSavingIntoDataBase_Then_AnErrorMessageIsReturned()
        {
            var homeworkRepository = mockHomeworkDataBaseInsertError();
            HomeworkService homeworkService = new HomeworkService(homeworkRepository);
            List<string> expectedUrls = new List<string>(new string[] { "test" });
            string messageToBePrinted = homeworkService.Upload(CreateUpload(expectedUrls), new List<IFormFile>(), "f3610178-09ca-497d-927f-3edff8ab2ea0", "f3610178-09ca-497d-927f-3edff8ab2ea1", "None");
            addedHomework.Count.Should().Be(0);
            string expectedErrorMessage = "Not uploaded files:test";
            messageToBePrinted.Should().Be(expectedErrorMessage);
        }

        [TestMethod]
        public void When_UploadIsCalledWithNullUploadHelper_Then_ArgumentNullExceptionIsThrown()
        {
            var homeworkRepository = mockHomeworkRepositoryExpectedBehaviour();
            HomeworkService homeworkService = new HomeworkService(homeworkRepository);
            try
            {
                homeworkService.Upload(null, new List<IFormFile>(), "f3610178-09ca-497d-927f-3edff8ab2ea0", "f3610178-09ca-497d-927f-3edff8ab2ea1", "None");
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void When_UploadIsCalledWithNullList_Then_ArgumentNullExceptionIsThrown()
        {
            var homeworkRepository = mockHomeworkRepositoryExpectedBehaviour();
            HomeworkService homeworkService = new HomeworkService(homeworkRepository);
            List<string> expectedUrls = new List<string>(new string[] { "test" });
            try
            {
                homeworkService.Upload(CreateUpload(expectedUrls), null, "f3610178-09ca-497d-927f-3edff8ab2ea0", "f3610178-09ca-497d-927f-3edff8ab2ea1", "None");
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void When_UploadIsCalledWithNullUid_Then_ArgumentNullExceptionIsThrown()
        {
            var homeworkRepository = mockHomeworkRepositoryExpectedBehaviour();
            HomeworkService homeworkService = new HomeworkService(homeworkRepository);
            List<string> expectedUrls = new List<string>(new string[] { "test" });
            try
            {
                homeworkService.Upload(CreateUpload(expectedUrls), new List<IFormFile>(), null, "f3610178-09ca-497d-927f-3edff8ab2ea1", "None");
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void When_UploadIsCalledWithNullMid_Then_ArgumentNullExceptionIsThrown()
        {
            var homeworkRepository = mockHomeworkRepositoryExpectedBehaviour();
            HomeworkService homeworkService = new HomeworkService(homeworkRepository);
            List<string> expectedUrls = new List<string>(new string[] { "test" });
            try
            {
                homeworkService.Upload(CreateUpload(expectedUrls), new List<IFormFile>(), "f3610178-09ca-497d-927f-3edff8ab2ea1", null, "None");
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void When_UploadIsCalledWithInvalidMid_Then_FormatErrorIsThown()
        {
            var homeworkRepository = mockHomeworkRepositoryExpectedBehaviour();
            HomeworkService homeworkService = new HomeworkService(homeworkRepository);
            List<string> expectedUrls = new List<string>(new string[] { "test" });
            try
            {
                homeworkService.Upload(CreateUpload(expectedUrls), new List<IFormFile>(), "f3610178-09ca-497d-927f-3edff8ab2ea1", "aaaaa", "None");
                Assert.Fail();
            }
            catch
            {
            }
        }

        [TestMethod]
        public void When_UploadIsCalledWithInvalidlUid_Then_FormatErrorIsThown()
        {
            var homeworkRepository = mockHomeworkRepositoryExpectedBehaviour();
            HomeworkService homeworkService = new HomeworkService(homeworkRepository);
            List<string> expectedUrls = new List<string>(new string[] { "test" });
            try
            {
                homeworkService.Upload(CreateUpload(expectedUrls), new List<IFormFile>(), "aaaa", "f3610178-09ca-497d-927f-3edff8ab2ea1", "None");
                Assert.Fail();
            }
            catch
            {
            }
        }
    }
}
