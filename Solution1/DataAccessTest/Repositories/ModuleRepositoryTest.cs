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
    public class ModuleRepositoryTest
    {
        private IModuleRepository _moduleRepository;
        private Module _testModule;
        private List<Module> expected_modules;
        private List<Module> expected_updated_modules;

        private Module InitModule(string id)
        {
            Guid module_id;
            if (id == null)
            {
                module_id = Guid.Empty;
            }
            else
            {
                module_id = Guid.Parse(id);
            }
            var newModule = new Module
            {
                Id = module_id,
                CourseId = Guid.Empty,
                Description = "Test Module",
                HasHomework = true,
                HasTest = false,
                Title = "Test Module",
                UrlPdf = "http://pdf.com"
            };
            return newModule;
        }

        private void putValusIntoMockedDbSet(Mock<DbSet<Module>> mockSet, List<Module> valuesToBeAssigned)
        {
            var data = valuesToBeAssigned.AsQueryable();
            mockSet.As<IQueryable<Module>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Module>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Module>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Module>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        }

        private DbSet<Module> mockDbSet()
        {
            expected_modules = new List<Module>
            {
                InitModule("eeeaccd6-f995-4a75-a5ce-02ee88bddab5"),
                InitModule("e6d70219-b169-4d74-8b30-4242b218a744")
            };
            expected_updated_modules = new List<Module>(expected_modules);
            expected_updated_modules[1].Description = "test";
            var data = expected_modules.AsQueryable();
            var mockSet = new Mock<DbSet<Module>>();
            putValusIntoMockedDbSet(mockSet, expected_modules);
            mockSet.Setup(d => d.Add(It.IsAny<Module>())).Callback<Module>((s) => data.Append(s));
            mockSet.Setup(d => d.Update(It.IsAny<Module>())).Callback<Module>((s) => putValusIntoMockedDbSet(mockSet, expected_updated_modules));
            return mockSet.Object;
        }

        private IPlatformManagement CreateSUT()
        {
            var mockPlatformManagement = new Mock<IPlatformManagement>();
            var modules = mockDbSet();
            mockPlatformManagement.Setup(m => m.Modules).Returns(modules);
            mockPlatformManagement.Setup(d => d.Set<Module>()).Returns(modules);
            return mockPlatformManagement.Object;
        }

        [TestMethod]
        public void When_TestModuleIsCreated_ShouldReturnItByFindAll()
        {
            IPlatformManagement platformManagement = CreateSUT();
            ModuleRepository moduleRepository = new ModuleRepository(platformManagement);

            var new_module = InitModule(null);

            moduleRepository.Create(new_module).Should().Be(new_module);
        }

        [TestMethod]
        public void When_TestModuleIsUpdated_ShouldReturnitByFindAll()
        {
            IPlatformManagement platformManagement = CreateSUT();
            ModuleRepository moduleRepository = new ModuleRepository(platformManagement);
            moduleRepository.Update(expected_updated_modules[1]);
            moduleRepository.GetAll().ShouldBeEquivalentTo(expected_updated_modules);
        }

        [TestMethod]
        public void When_GetByIdIsCalled_Then_TheCorespondingModuleIsReturned()
        {
            IPlatformManagement platformManagement = CreateSUT();
            ModuleRepository moduleRepository = new ModuleRepository(platformManagement);

            Module result = moduleRepository.GetById(Guid.Parse("eeeaccd6-f995-4a75-a5ce-02ee88bddab5"));
            result.Should().Be(expected_modules[0]);
        }

        [TestMethod]
        public void When_GetAllIsCalled_Then_AllTheObjectsInDataBaseAreReturned()
        {
            IPlatformManagement platformManagement = CreateSUT();
            ModuleRepository moduleRepository = new ModuleRepository(platformManagement);

            List<Module> all = moduleRepository.GetAll().ToList();
            all.Should().BeEquivalentTo(expected_modules);
        }        
    }
}
