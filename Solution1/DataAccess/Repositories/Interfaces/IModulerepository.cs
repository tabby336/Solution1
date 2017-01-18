using System;
using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IModuleRepository : IRepository<Module>
    {
        Module GetByIdWithCourse(Guid id);
    }
}
