using System;
using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPlayerRepository : IRepository<Player>
    {
        Player GetByIdWithCourses(Guid id, bool includeModules = false);
    }
}
