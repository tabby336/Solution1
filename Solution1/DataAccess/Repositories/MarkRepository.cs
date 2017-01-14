using DataAccess.Repositories.Interfaces;
using DataAccess.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repositories
{
    public class MarkRepository : Repository<Mark>, IMarkRepository
    {
        public MarkRepository(IPlatformManagement platformManagement) : base(platformManagement)
        {
        }

        public IEnumerable<Mark> GetMarksByUserId(Guid uid) 
        {
            return context.Marks.Where(mark => mark.UserId == uid);
        }
    }
}
