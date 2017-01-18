using System.Collections.Generic;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;

namespace Business.Services.Interfaces
{
    public interface ICourseService
    {
        IEnumerable<Course> GetAllCourses(bool includeModules = false);
        IEnumerable<Course> GetCoursesForPlayer(string playerId, bool includeModules = false);
        IEnumerable<Course> GetCoursesForAuthor(string authorName);
        Course CreateCourse(string userid, string title, string description, string hashtag, string datalink, IList<IFormFile> files);
        string GetImagePathForCourseId(string id);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);
        void Partikip(string userId, string courseId);
    }
}