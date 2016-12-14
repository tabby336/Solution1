using DataAccess.Repositories.Interfaces;
using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class MarkRepository : Repository<Mark>, IMarkRepository
    {
        public MarkRepository(PlatformManagement platformManagement) : base(platformManagement)
        {
        }
    }
}
