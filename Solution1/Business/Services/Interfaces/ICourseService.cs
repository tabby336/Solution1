using System.Collections.Generic;
using DataAccess.Models;

namespace Business.Services.Interfaces
{
    public interface ICourseService
    {
        IEnumerable<Course> GetAllCourses(bool includeModules = false);
        IEnumerable<string> GetAllCourseNames();
        Course CreateCourse(Course course, Player creator);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);

    }
}