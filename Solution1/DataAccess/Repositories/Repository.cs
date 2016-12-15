using DataAccess.Repositories.Interfaces;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repositories
{
    public abstract class Repository<T> : IRepository<T> 
    where T : ModelBase
    {
        protected readonly PlatformManagement context;
         
        protected Repository(PlatformManagement platformManagement)
        {
            context = platformManagement;
        }

        public T Create(T p)
        {
            context.Add(p);
            context.SaveChanges();
            return p;
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

        public T GetById(Guid id)
        {
            var queryResult = from R in context.Set<T>()
                                where R.Id == id
                                select R;
            return queryResult.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            var queryResult = from R in context.Set<T>()
                                select R;
            return queryResult;
        }
    }
}
