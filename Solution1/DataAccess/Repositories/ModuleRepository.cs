using System;
using System.Linq;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        private readonly IPlatformManagement _platformManagement;

        public ModuleRepository(IPlatformManagement platformManagement) : base(platformManagement)
        {
            _platformManagement = platformManagement;
        }

        public Module GetByIdWithCourse(Guid id)
        {
            return _platformManagement.Modules.Where(m => m.Id == id).Include(m => m.Course).FirstOrDefault();
        }
    }
}
