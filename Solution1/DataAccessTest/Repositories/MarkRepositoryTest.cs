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
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DataAccessTest.Repositories
{
    [TestClass]
    public class MarkRepositoryTest
    {
        private Mark InitMark(string id, string uid, string hid, float val)
        {
            Mark mark = new Mark();
            mark.Id = Guid.Parse(id);
            mark.HomeworkId = Guid.Parse(hid);
            mark.UserId = Guid.Parse(uid);
            mark.Value = val;
            mark.Description = null;
            mark.Timestamp = new DateTime(1000);
            mark.CreatorId = Guid.Parse(hid);
            mark.HasComment = false;
            mark.HasContestation = false;
            return mark;
        }

        private DbSet<Mark> mockDbSet()
        {
            var data = new List<Mark>
            {
                InitMark("eeeaccd6-f995-4a75-a5ce-02ee88bddab5",
                         "89bd83e6-8756-4c66-912f-2f065b4cc8dc",
                         "a9ca496a-849f-4575-bc65-088e87ee92e1",
                         7),
                InitMark("e6d70219-b169-4d74-8b30-4242b218a744",
                         "7cc73a6e-9a93-4d6c-a96a-ef753b9054dd",
                         "f3610178-09ca-497d-927f-3edff8ab2ea0",
                         8)
            }.AsQueryable();
            var mockSet = new Mock<DbSet<Mark>>();
            mockSet.As<IQueryable<Mark>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Mark>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Mark>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Mark>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet.Object;
        }

        private IPlatformManagement CreateSUT()
        {
            var mockPlatformManagement = new Mock<IPlatformManagement>();
            mockPlatformManagement.Setup(m => m.Marks).Returns(mockDbSet());
            return mockPlatformManagement.Object;
        }

        [TestMethod]
        public void When_GetMarksByUserIdIsCalled_Then_ShouldReturnExpectedMark()
        {
            IPlatformManagement platformManagement = CreateSUT();
            MarkRepository _markRepository = new MarkRepository(platformManagement);
            var expected_result = platformManagement.Marks.Where(mark => mark.UserId == Guid.Parse("89bd83e6-8756-4c66-912f-2f065b4cc8dc"));
            List<Mark> marks = _markRepository.GetMarksByUserId(Guid.Parse("89bd83e6-8756-4c66-912f-2f065b4cc8dc")).ToList();
            marks.Should().BeEquivalentTo(expected_result);
        }
    }
}