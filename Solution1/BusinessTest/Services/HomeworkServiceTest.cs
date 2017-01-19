using System;
using System.Collections.Generic;
using Business.CommonInfrastructure.Interfaces;
using Business.Services;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessTest.Services
{
    [TestClass]
    public class HomeworkServiceTest
    {
        private List<Homework> _addedHomework;

        private static Homework HomeworkInit(string uid, string mid, string obs)
        {
            return new Homework
            {
                ModuleId = Guid.Parse(mid),
                UserId = Guid.Parse(uid),
                Timestamp = new DateTime(10000),
                Observations = obs
            };
        }

        private static IUpload CreateUpload(IList<string> pathList)
        {
            var mockUpload = new Mock<IUpload>();
            mockUpload.Setup(m => m.UploadFiles(It.IsAny<IList<IFormFile>>(), It.IsAny<string>())).Returns(pathList);
            return mockUpload.Object;
        }

        private IHomeworkRepository MockHomeworkRepositoryExpectedBehaviour()
        {
            var mockHomeworkRepository = new Mock<IHomeworkRepository>();
            _addedHomework = new List<Homework>();
            mockHomeworkRepository.Setup(m => m.Create(It.IsAny<Homework>())).Callback<Homework>(s => _addedHomework.Add(s)).Returns(new Homework());
            return mockHomeworkRepository.Object;
        }

        private IPlayerRepository mockEmptyPlayerRepository()
        {
            var mockPlayerRepository = new Mock<IPlayerRepository>();
            return mockPlayerRepository.Object;
        }

        private IHomeworkRepository MockHomeworkDataBaseInsertError()
        {
            var mockHomeworkRepository = new Mock<IHomeworkRepository>();
            _addedHomework = new List<Homework>();
            mockHomeworkRepository.Setup(m => m.Create(It.IsAny<Homework>())).Returns((Homework)null);
            return mockHomeworkRepository.Object;
        }

        private IEnumerable<Homework> CreateHomeworkListForModul(Guid moduleId)
        {
            var homework1 = HomeworkInit("f3610178-09ca-497d-927f-3edff8ab2ea2", moduleId.ToString(), "None");
            var homework2 = HomeworkInit("f3610178-09ca-497d-927f-3edff8ab2ea2", moduleId.ToString(), "None");
            var homework3 = HomeworkInit("f3610178-09ca-497d-927f-3edff8ab2ea3", moduleId.ToString(), "None");
            return new List<Homework>() { homework1, homework2, homework3 };
        }

        [TestMethod]
        public void When_GetPlayersThatUploadedIsCalled_Then_TheListIsRetuned()
        {
            var expectedPlayer = new List<Player>() { new Player() { Id = Guid.Parse("f3610178-09ca-497d-927f-3edff8ab2ea2") }, new Player() { Id = Guid.Parse("f3610178-09ca-497d-927f-3edff8ab2ea3") } };
            var moduleId = Guid.Parse("f3610178-09ca-497d-927f-3edff8ab2ea1");
            var mockHomeworkRepository = new Mock<IHomeworkRepository>();
            mockHomeworkRepository.Setup(m => m.GetHomeworksByModuleId(moduleId)).Returns(CreateHomeworkListForModul(moduleId));
            var mockPlayerRepository = new Mock<IPlayerRepository>();
            mockPlayerRepository.Setup(m => m.GetById(Guid.Parse("f3610178-09ca-497d-927f-3edff8ab2ea2"))).Returns(expectedPlayer[0]);
            mockPlayerRepository.Setup(m => m.GetById(Guid.Parse("f3610178-09ca-497d-927f-3edff8ab2ea3"))).Returns(expectedPlayer[1]);

            var homeworkService = new HomeworkService(mockHomeworkRepository.Object, mockPlayerRepository.Object);

            homeworkService.GetPlayersThatUploaded(moduleId.ToString()).Should().BeEquivalentTo(expectedPlayer);
        }

        [TestMethod]
        public void When_UploadIsCalled_Then_TheFilesAreSavedIntoDatabase()
        {
            var homeworkRepository = MockHomeworkRepositoryExpectedBehaviour();
            var playerRepository = mockEmptyPlayerRepository();
            var homeworkService = new HomeworkService(homeworkRepository, playerRepository);
            var expectedUrls = new List<string>(new[] { "test1", "test2" });
            var messageToBePrinted = homeworkService.Upload(CreateUpload(expectedUrls), new List<IFormFile>(), "f3610178-09ca-497d-927f-3edff8ab2ea0", "f3610178-09ca-497d-927f-3edff8ab2ea1", "None");
            for (var i = 0; i < _addedHomework.Count; ++i)
            {
                var homework = _addedHomework[i];
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
            var homeworkRepository = MockHomeworkDataBaseInsertError();
            var playerRepository = mockEmptyPlayerRepository();
            var homeworkService = new HomeworkService(homeworkRepository, playerRepository);
            var expectedUrls = new List<string>(new[] { "test" });
            var messageToBePrinted = homeworkService.Upload(CreateUpload(expectedUrls), new List<IFormFile>(), "f3610178-09ca-497d-927f-3edff8ab2ea0", "f3610178-09ca-497d-927f-3edff8ab2ea1", "None");
            _addedHomework.Count.Should().Be(0);
            const string expectedErrorMessage = "Not uploaded files:test";
            messageToBePrinted.Should().Be(expectedErrorMessage);
        }

        [TestMethod]
        public void When_UploadIsCalledWithNullUploadHelper_Then_ArgumentNullExceptionIsThrown()
        {
            var homeworkRepository = MockHomeworkRepositoryExpectedBehaviour();
            var playerRepository = mockEmptyPlayerRepository();
            var homeworkService = new HomeworkService(homeworkRepository, playerRepository);
            try
            {
                homeworkService.Upload(null, new List<IFormFile>(), "f3610178-09ca-497d-927f-3edff8ab2ea0", "f3610178-09ca-497d-927f-3edff8ab2ea1", "None");
                Assert.Fail();
            }
            catch
            {
                // ignored
            }
        }

        [TestMethod]
        public void When_UploadIsCalledWithNullList_Then_ArgumentNullExceptionIsThrown()
        {
            var homeworkRepository = MockHomeworkRepositoryExpectedBehaviour();
            var playerRepository = mockEmptyPlayerRepository();
            var homeworkService = new HomeworkService(homeworkRepository, playerRepository);
            var expectedUrls = new List<string>(new[] { "test" });
            try
            {
                homeworkService.Upload(CreateUpload(expectedUrls), null, "f3610178-09ca-497d-927f-3edff8ab2ea0", "f3610178-09ca-497d-927f-3edff8ab2ea1", "None");
                Assert.Fail();
            }
            catch
            {
                // ignored
            }
        }

        [TestMethod]
        public void When_UploadIsCalledWithNullUid_Then_ArgumentNullExceptionIsThrown()
        {
            var homeworkRepository = MockHomeworkRepositoryExpectedBehaviour();
            var playerRepository = mockEmptyPlayerRepository();
            var homeworkService = new HomeworkService(homeworkRepository, playerRepository);
            var expectedUrls = new List<string>(new[] { "test" });
            try
            {
                homeworkService.Upload(CreateUpload(expectedUrls), new List<IFormFile>(), null, "f3610178-09ca-497d-927f-3edff8ab2ea1", "None");
                Assert.Fail();
            }
            catch
            {
                // ignored
            }
        }

        [TestMethod]
        public void When_UploadIsCalledWithNullMid_Then_ArgumentNullExceptionIsThrown()
        {
            var homeworkRepository = MockHomeworkRepositoryExpectedBehaviour();
            var playerRepository = mockEmptyPlayerRepository();
            var homeworkService = new HomeworkService(homeworkRepository, playerRepository);
            var expectedUrls = new List<string>(new[] { "test" });
            try
            {
                homeworkService.Upload(CreateUpload(expectedUrls), new List<IFormFile>(), "f3610178-09ca-497d-927f-3edff8ab2ea1", null, "None");
                Assert.Fail();
            }
            catch
            {
                // ignored
            }
        }

        [TestMethod]
        public void When_UploadIsCalledWithInvalidMid_Then_FormatErrorIsThown()
        {
            var homeworkRepository = MockHomeworkRepositoryExpectedBehaviour();
            var playerRepository = mockEmptyPlayerRepository();
            var homeworkService = new HomeworkService(homeworkRepository, playerRepository);
            var expectedUrls = new List<string>(new[] { "test" });
            try
            {
                homeworkService.Upload(CreateUpload(expectedUrls), new List<IFormFile>(), "f3610178-09ca-497d-927f-3edff8ab2ea1", "aaaaa", "None");
                Assert.Fail();
            }
            catch
            {
                // ignored
            }
        }

        [TestMethod]
        public void When_UploadIsCalledWithInvalidlUid_Then_FormatErrorIsThown()
        {
            var homeworkRepository = MockHomeworkRepositoryExpectedBehaviour();
            var playerRepository = mockEmptyPlayerRepository();
            var homeworkService = new HomeworkService(homeworkRepository, playerRepository);
            var expectedUrls = new List<string>(new[] { "test" });
            try
            {
                homeworkService.Upload(CreateUpload(expectedUrls), new List<IFormFile>(), "aaaa", "f3610178-09ca-497d-927f-3edff8ab2ea1", "None");
                Assert.Fail();
            }
            catch
            {
                // ignored
            }
        }
    }
}
