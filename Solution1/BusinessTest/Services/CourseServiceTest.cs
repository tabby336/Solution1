using Business.CommonInfrastructure;
using Business.CommonInfrastructure.Interfaces;
using Business.Services;
using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BusinessTest.Services
{
    [TestClass]
    public class CourseServiceTest
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

        private ICollection<Module> CreateModulesList(List<Guid> modulesId)
        {
            List<Module> modules = new List<Module>();
            foreach (var module in modulesId)
            {
                modules.Add(new Module() { Id = module });
            }

            return new Collection<Module>(modules);
        }

        private List<Course> CreateCourseList()
        {
            List<Course> courses = new List<Course>()
            {
                CreateCourse(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054aa"),
                             CreateModulesList(new List<Guid> {
                                 Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054a0"),
                                 Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054a1")
                             }), "test"),
                CreateCourse(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054bb"),
                             CreateModulesList(new List<Guid> {
                                 Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054b0"),
                             }), "test2")
            };
            return courses;
        }


        [TestMethod]
        public void When_GetAllCoursesIsCalledWithDefaultParameter_Then_AllCoursesAreReturned()
        {
            List<Course> courses = CreateCourseList();
            Mock<ICourseRepository> courseRepository = new Mock<ICourseRepository>();
            courseRepository.Setup(m => m.GetAll()).Returns(courses);

            CourseService courseService = new CourseService(courseRepository.Object, null, null);

            courseService.GetAllCourses().Should().Equal(courses);
        }

        [TestMethod]
        public void When_GetAllCoursesIsCalledWithTrueParameter_Then_AllCoursesAreReturned()
        {
            List<Course> courses = CreateCourseList();
            Mock<ICourseRepository> courseRepository = new Mock<ICourseRepository>();
            courseRepository.Setup(m => m.GetAllWithModules()).Returns(courses);

            CourseService courseService = new CourseService(courseRepository.Object, null, null);

            courseService.GetAllCourses(true).Should().Equal(courses);
        }

        [TestMethod]
        public void When_DeleteIsCalled_Then_IsDeletedOnlyTheRequestedCourse()
        {
            List<Course> courses = CreateCourseList();
            Mock<ICourseRepository> courseRepository = new Mock<ICourseRepository>();
            courseRepository.Setup(m => m.Delete(courses[1])).Callback<Course>((s) => courses.Remove(s));

            CourseService courseService = new CourseService(courseRepository.Object, null, null);

            courseService.DeleteCourse(courses[1]);
            courses.Should().Equal(new List<Course> {courses[0]});
        }

        [TestMethod]
        public void When_UpdateIsCalled_Then_IsUpdatedOnlyTheRequestedCourse()
        {
            List<Course> courses = CreateCourseList();
            Course updatedCourse = courses[0];
            updatedCourse.Title = "Title";
            courses[0] = updatedCourse;
            Mock<ICourseRepository> courseRepository = new Mock<ICourseRepository>();
            courseRepository.Setup(m => m.Update(updatedCourse)).Callback<Course>((s) => courses[0] = s);

            CourseService courseService = new CourseService(courseRepository.Object, null, null);

            courseService.DeleteCourse(courses[1]);
            courses.Should().Equal(new List<Course> { updatedCourse, courses[1] });
        }

        [TestMethod]
        public void When_UploadIsCalledWithNullUpload_Then_ReturnsFalse()
        {
            CourseService courseService = new CourseService(null, null, null);
            courseService.Upload(null, new List<IFormFile>(), "7cc73a6e-9a93-4d6c-a96a-ef753b9054aa").Should().Be(false);
        }

        [TestMethod]
        public void When_UploadIsCalledWithNullCourseId_Then_ReturnsFalse()
        {
            CourseService courseService = new CourseService(null, null, null);
            courseService.Upload(new Upload(new FileDataSource()), new List<IFormFile>(), null).Should().Be(false);
        }

        [TestMethod]
        public void When_UploadWithoutErrors_Then_ReturnTrue()
        {
            CourseService courseService = new CourseService(null, null, null);
            Mock<IList<IFormFile>> fileList = new Mock<IList<IFormFile>>();
            fileList.Setup(m => m.Count).Returns(2);
            Mock<IUpload> upload = new Mock<IUpload>();
            upload.Setup(m => m.UploadFiles(fileList.Object, It.IsAny<string>())).Returns(new List<string> { "test1", "test2" });
            courseService.Upload(upload.Object, fileList.Object, "7cc73a6e-9a93-4d6c-a96a-ef753b9054aa").Should().Be(true);
        }

        [TestMethod]
        public void When_UploadWithErrors_Then_ReturnFalse()
        {
            CourseService courseService = new CourseService(null, null, null);
            Mock<IList<IFormFile>> fileList = new Mock<IList<IFormFile>>();
            fileList.Setup(m => m.Count).Returns(2);
            Mock<IUpload> upload = new Mock<IUpload>();
            upload.Setup(m => m.UploadFiles(fileList.Object, It.IsAny<string>())).Returns(new List<string> { "test"});
            courseService.Upload(upload.Object, fileList.Object, "7cc73a6e-9a93-4d6c-a96a-ef753b9054aa").Should().Be(false);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void When_PatiPartikipIsCalledAnTheStudentAlreadySubscribed_Then_AnExceptionIsThwron()
        {
            List<Course> courses = CreateCourseList();
            Player player = new Player { Id = Guid.Parse("7cc73a6e-9a93-1111-a96a-ef753b9054aa") };

            Mock<IPlayerService> playerService = new Mock<IPlayerService>();
            playerService.Setup(m => m.GetPlayerData("7cc73a6e-9a93-1111-a96a-ef753b9054aa", true, false)).Returns(player);

            Mock<ICourseRepository> courseRepository = new Mock<ICourseRepository>();
            courseRepository.Setup(m => m.GetById(Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054aa"))).Returns(courses[0]);

            Mock<IPlayerCourseRepository> playerCourseRepository = new Mock<IPlayerCourseRepository>();
            playerCourseRepository.Setup(m => m.GetByPlayerAndCourse(Guid.Parse("7cc73a6e-9a93-1111-a96a-ef753b9054aa"), 
                Guid.Parse("7cc73a6e-9a93-4d6c-a96a-ef753b9054aa"))).Returns(new PlayerCourse());

            CourseService courseService = new CourseService(courseRepository.Object, playerService.Object, playerCourseRepository.Object);
           
            courseService.Partikip("7cc73a6e-9a93-1111-a96a-ef753b9054aa", "7cc73a6e-9a93-4d6c-a96a-ef753b9054aa");
        }
    }
}
