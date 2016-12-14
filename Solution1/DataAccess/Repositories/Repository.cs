using DataAccess.Repositories.Interfaces;
using DataAccess.Models;
using System;
using Services.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public abstract class Repository<T> : IRepository<T> 
    where T : Mark
    {
        protected readonly PlatformManagement context;

        public Repository(PlatformManagement platformManagement)
        {
            context = platformManagement;
        }

        public void Create(T p)
        {
            context.Add(p);
            context.SaveChanges();
        }

        public void Update(T p)
        {
            context.Update(p);
            context.SaveChanges();
        }

        public void Delete(T p)
        {
            context.Remove(p);
            context.SaveChanges();
        }

        public IEnumerable<T> GetById(Guid id)
        {
            var queryResult = from R in context.Set<T>()
                                where R.UserId == id
                                select R;
            return queryResult;
        }

        public IEnumerable<T> GetAll()
        {
            var queryResult = from R in context.Set<T>()
                                select R;
            return queryResult;
        }
    }
}
