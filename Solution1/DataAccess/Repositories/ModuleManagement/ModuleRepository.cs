namespace DataAccess.Repositories.ModuleManagement
{
    public class ModuleRepository : Repository<Module>
    {
        public ModuleRepository(PlatformManagement platformManagement) : base(platformManagement)
        {
        }
    }
}
