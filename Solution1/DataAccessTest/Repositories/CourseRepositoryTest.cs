using System;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.ObjectModel;

namespace DataAccessTest.Repositories
{
    [TestClass]
    public class CourseRepositoryTest
    {
        private Course CreateCourse(Guid courseId, ICollection<Module> moduleIds, string name)
        {
            return new Course
            {
                Id = courseId,
                Modules = moduleIds,
                Title = name
            };
        }


        private DbSet<Course> mockDbSet(IQueryable<Course> anouncements)
        {
            var mockSet = new Mock<DbSet<Course>>();
            mockSet.As<IQueryable<Course>>().Setup(m => m.Provider).Returns(anouncements.Provider);
            mockSet.As<IQueryable<Course>>().Setup(m => m.Expression).Returns(anouncements.Expression);
            mockSet.As<IQueryable<Course>>().Setup(m => m.ElementType).Returns(anouncements.ElementType);
            mockSet.As<IQueryable<Course>>().Setup(m => m.GetEnumerator()).Returns(anouncements.GetEnumerator());
            return mockSet.Object;
        }

        private ICollection<Module> CreateModulesList(List<Guid> modulesId)
        {
            List<Module> modules = new List<Module>();
            foreach (var module in modulesId)
            {
                modules.Add(new Module() { Id = module });
            }

            return new Collection<Module>(modules);
        }

        private IPlatformManagement CreateSUT()
        {
            IQueryable<Course> courses = new List<Course>()
            {
                CreateCourse(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054aa"),
                             CreateModulesList(new List<Guid> {
                                 Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054a0"),
                                 Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054a1")
                             }), "test"),
                CreateCourse(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054bb"),
                             CreateModulesList(new List<Guid> {
                                 Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054b0"),
                             }), "test2"),
                CreateCourse(Guid.Parse("7cc72a61-9a93-4d6c-a96a-ef753b9054cc"),
                             CreateModulesList(new List<Guid> {}), "test3"),
            }.AsQueryable();

            Mock<IPlatformManagement> platformManagement = new Mock<IPlatformManagement>();
            platformManagement.Setup(m => m.Courses).Returns(mockDbSet(courses));

            return platformManagement.Object;
        }

        [TestMethod]
        public void When_GetCourseNamesIsCalled_Then_AListWithAllCoursesNameIsReturned()
        {
            IPlatformManagement platformManagement = CreateSUT();
            ICourseRepository courseRepository = new CourseRepository(platformManagement);
            List<string> expected_names = new List<string> { "test", "test2", "test3" };
            courseRepository.GetCourseNames().Should().Equal(expected_names);
        }
    }
}
