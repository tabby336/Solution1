using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class ModuleRepository : Repository<Module>
    {
        public ModuleRepository(PlatformManagement platformManagement) : base(platformManagement)
        {
        }
    }
}
