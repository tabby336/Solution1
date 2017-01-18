
using System;
using System.Linq;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        private readonly IPlatformManagement _platformManagement;

        public PlayerRepository(IPlatformManagement platformManagement) : base(platformManagement)
        {
            _platformManagement = platformManagement;
        }
    }
}
