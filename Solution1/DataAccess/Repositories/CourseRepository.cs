using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private readonly IPlatformManagement _platformManagement;
        public CourseRepository(IPlatformManagement platformManagement) : base(platformManagement)
        {
           this._platformManagement = platformManagement;
        }

        public IEnumerable<Course> GetAllWithModules()
        {
            return _platformManagement.Courses.Include(course => course.Modules).ToList();
        }

        public IEnumerable<string> GetCourseNames()
        {
            return _platformManagement.Courses.Select(x => x.Title).ToList();
        }

        public IEnumerable<Course> GetCoursesByAuthor(string authorName)
        {
            return _platformManagement.Courses.Where(x => x.Author == authorName).ToList();
        }

        public IEnumerable<Module> GetModulesForCourse(Guid courseId)
        {
            var course = _platformManagement.Courses.Where(c => c.Id == courseId).Include(c => c.Modules).FirstOrDefault();
            if(course == null) return new List<Module>();
            return course.Modules;
        }
    }
}
