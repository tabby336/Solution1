using System;
using System.Collections.Generic;
using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        IEnumerable<Course> GetAllWithModules();
        IEnumerable<string> GetCourseNames();
        IEnumerable<Module> GetModulesForCourse(Guid courseId);
        IEnumerable<Course> GetCoursesByAuthor(string authorName);
    }
}
