using System.Collections.Generic;
using DataAccess.Models;

namespace Business.Services.Interfaces
{
    public interface ICourseService
    {
        IList<Course> GetAllCourses();
        IList<string> GetAllCourseNames();
        void CreateCourse(Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);

    }
}