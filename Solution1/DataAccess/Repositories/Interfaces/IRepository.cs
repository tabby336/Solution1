using System;
using System.Collections.Generic;

namespace DataAccess.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        T Create(T obj);
        void Update(T obj);
        void Delete(T obj);
        T GetById(Guid id);
        IEnumerable<T> GetAll();
    }
}