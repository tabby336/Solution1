using System.Collections.Generic;
using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        IEnumerable<Course> GetAllWithModules();
        IEnumerable<string> GetCourseNames();
    }
}
