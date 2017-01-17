using System.Collections.Generic;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;

namespace Business.Services.Interfaces
{
    public interface ICourseService
    {
        IEnumerable<Course> GetAllCourses(bool includeModules = false);
        IEnumerable<string> GetAllCourseNames();
        Course CreateCourse(string userid, string title, string description, string hashtag, string datalink, IList<IFormFile> files);
        string GetImagePathForCourseId(string id);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);

    }
}