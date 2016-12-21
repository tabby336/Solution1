using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private PlatformManagement platformManagement;
        public CourseRepository(PlatformManagement platformManagement) : base(platformManagement)
        {
           this.platformManagement = platformManagement;
        }

        public IList<string> GetCourseNames()
        {
            return platformManagement.Courses.Select(x => x.Title).ToList();
        } 
    }
}
