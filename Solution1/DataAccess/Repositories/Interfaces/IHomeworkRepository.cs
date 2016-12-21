using DataAccess.Models;

using System;
using System.Collections.Generic;
namespace DataAccess.Repositories.Interfaces
{
   public interface IHomeworkRepository : IRepository<Homework>
    {
        IEnumerable<Homework> GetHomeworksByUserId(Guid uid);
    }
}
