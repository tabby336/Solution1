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
        private readonly IPlayerService _playerService;

        public CourseService(ICourseRepository courseRepository, IPlayerService playerService)
        {
            _courseRepository = courseRepository;
            _playerService = playerService;
        }

        public IEnumerable<Course> GetAllCourses(bool includeModules = false)
        {
            return includeModules ? _courseRepository.GetAllWithModules() : _courseRepository.GetAll().ToList();
        }

        public IEnumerable<string> GetAllCourseNames()
        {
            return _courseRepository.GetCourseNames();
        }

        public Course CreateCourse(Course course, Player creator)
        {
            try
            {
                var created = _courseRepository.Create(course);
                creator.Courses.Add(created);
                _playerService.UpdatePlayer(creator);
                return created;
            }
            catch (Exception)
            {
                return null;
            }
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

