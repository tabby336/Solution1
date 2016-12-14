using System;
using System.Collections.Generic;

namespace DataAccess.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        void Create(T obj);
        void Update(T obj);
        void Delete(T obj);
        IEnumerable<T> GetById(Guid id);
        IEnumerable<T> GetAll();
    }
}