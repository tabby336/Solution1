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
    }
}
