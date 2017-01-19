using System.Collections.Generic;
using DataAccess.Models;

namespace Web.Models.CourseViewModels
{
    public class CourseViewModel
    {
        public List<Course> Courses { get; set; }
        public List<Course> MyCourses { get; set; }
       
    }
}
