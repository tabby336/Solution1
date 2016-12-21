using System.Collections.Generic;
using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        IList<string> GetCourseNames();
    }
}
