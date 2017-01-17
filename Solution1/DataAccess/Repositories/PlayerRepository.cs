
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        public PlayerRepository(IPlatformManagement platformManagement) : base(platformManagement)
        {
        }
    }
}
