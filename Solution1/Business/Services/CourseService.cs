using System;
using System.Collections.Generic;
using System.Linq;
using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace Business.Services
{

    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public IEnumerable<Course> GetAllCourses(bool includeModules = false)
        {
            return includeModules ? _courseRepository.GetAllWithModules() : _courseRepository.GetAll().ToList();
        }

        public IEnumerable<string> GetAllCourseNames()
        {
            return _courseRepository.GetCourseNames();
        }

        public void CreateCourse(Course course)
        {
            _courseRepository.Create(course);
        }

        public void UpdateCourse(Course course)
        {
            _courseRepository.Update(course);
        }

        public void DeleteCourse(Course course)
        {
            _courseRepository.Delete(course);
        }
    }
}
