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
    public class ModuleRepositoryTest
    {
        private IModuleRepository _moduleRepository;
        private Module _testModule;

        private Module CreateSUT()
        {
            Module newModule = new Module();
            newModule.Id = Guid.Parse("0842f5b0-0ea3-4e16-143a-08d424682a17");
            newModule.CourseId = Guid.Empty;
            newModule.Description = "Test Module";
            newModule.HasHomework = true;
            newModule.HasTest = false;
            newModule.Title = "Test Module";
            newModule.UrlPdf = "http://pdf.com";
            return newModule;
        }

        [TestInitialize]
        public void SetUp()
        {
            PlatformManagement dbContext = new PlatformManagement();
            _moduleRepository = new ModuleRepository(dbContext);
            _testModule = CreateSUT();
        }

        [TestCleanup]
        public void TearDown()
        {
            _moduleRepository = null;
        }

        [TestMethod]
        public void When_TestModuleIsCreated_ShouldReturnItByFindAll()
        {

            _testModule = _moduleRepository.Create(_testModule);

            List<Module> all = _moduleRepository.GetAll().ToList();
            all.Should().Contain(_testModule);
        }

        [TestMethod]
        public void When_TestModuleIsUpdated_ShouldReturnitByFindAll()
        {

            _testModule.Description = "Updated module";
            _moduleRepository.Update(_testModule);

            List<Module> all = _moduleRepository.GetAll().ToList();
            all.Should().Contain(_testModule);
        }

        [TestMethod]
        public void When_SearchingByTestModuleId_ShouldReturnTestModule()
        {
            Module result = _moduleRepository.GetById(Guid.Parse("0842f5b0-0ea3-4e16-143a-08d424682a17"));

            result.Id.Should().Be(_testModule.Id);
        }

        [TestMethod]
        public void When_TestModuleIsDeleted_ShouldNotReturnItByFindAll()
        {
            _moduleRepository.Delete(_testModule);

            List<Module> all = _moduleRepository.GetAll().ToList();
            all.Should().NotContain(_testModule);
        }        
    }
}
