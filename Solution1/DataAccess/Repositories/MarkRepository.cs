using DataAccess.Repositories.Interfaces;
using DataAccess.Models;
using Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class MarkRepository : Repository<Mark>, IMarkRepository
    {
        public MarkRepository(PlatformManagement platformManagement) : base(platformManagement)
        {
        }
    }
}
