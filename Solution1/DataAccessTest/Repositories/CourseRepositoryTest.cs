using System;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessTest.Repositories
{
    [TestClass]
    public class ExampleClass
    {
        private ICourseRepository _courseRepository;
        private IModuleRepository _moduleRepository;

        [TestInitialize]
        public void SetUp()
        {
            var dbContext = new PlatformManagement();
            _courseRepository = new CourseRepository(dbContext);
            _moduleRepository = new ModuleRepository(dbContext);
        }

        [TestCleanup]
        public void TearDown()
        {
            _courseRepository = null;
        }

        [TestMethod]
        public void When_ThisMethodRuns_NothingHappens()
        {
            Assert.AreEqual(true, true);
        }
    }
}
