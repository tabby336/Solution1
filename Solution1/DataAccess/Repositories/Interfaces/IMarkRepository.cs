using DataAccess.Models;

using System;
using System.Collections.Generic;

namespace DataAccess.Repositories.Interfaces
{
    public interface IMarkRepository : IRepository<Mark>
    {
        IEnumerable<Mark> GetMarksByUserId(Guid uid);
    }
}