using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories
{
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        public ModuleRepository(IPlatformManagement platformManagement) : base(platformManagement)
        {
        }
    }
}
