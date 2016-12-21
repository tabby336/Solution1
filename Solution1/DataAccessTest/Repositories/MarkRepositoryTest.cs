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
    public class MarkRepositoryTest
    {
        private IMarkRepository _markRepository;
        private IEnumerable<Mark> _marks;
        private string _expectedGuid = "89bd83e6-8756-4c66-912f-2f065b4cc8dc";
        private float _expectedMark = 7;

        private Mark InitMark(string id, string uid, string hid, float val)
        {
            Mark mark = new Mark();
            mark.Id = Guid.Parse(id);
            mark.HomeworkId = Guid.Parse(hid);
            mark.UserId = Guid.Parse(uid);
            mark.Value = val;
            mark.Description = null;
            mark.Timestamp = DateTime.Now;
            mark.CreatorId = Guid.Parse(hid);
            mark.HasComment = false;
            mark.HasContestation = false;
            return mark;
        }

        private IEnumerable<Mark> CreateSUT()
        {
            return new List<Mark>()
            {
                InitMark("eeeaccd6-f995-4a75-a5ce-02ee88bddab5",
                         _expectedGuid,
                         "a9ca496a-849f-4575-bc65-088e87ee92e1",
                         7),
                InitMark("e6d70219-b169-4d74-8b30-4242b218a744",
                         "7cc73a6e-9a93-4d6c-a96a-ef753b9054dd",
                         "f3610178-09ca-497d-927f-3edff8ab2ea0",
                         8)
            };
        } 

        [TestInitialize]
        public void SetUp()
        {
            PlatformManagement dbContext = new PlatformManagement();
            _markRepository = new MarkRepository(dbContext);

            _marks = CreateSUT();        
            foreach(Mark m in _marks)
            {
                _markRepository.Create(m);
            }
        }

        [TestCleanup]
        public void TearDown()
        {   
            foreach(Mark m in _marks)
            {
                _markRepository.Delete(m);
            }
            _markRepository = null;
        }

        [TestMethod]
        public void When_GetMarksByUserIdIsCalled_Then_ShouldReturnExpectedMark()
        {
            List<Mark> marks = _markRepository.GetMarksByUserId(Guid.Parse(_expectedGuid)).ToList();
            marks.Count().Should().Be(1);
            marks[0].Value.Should().Be(_expectedMark);
        }
    }
}