using System;
using System.Collections.Generic;

namespace WasteManagement.Models.Interfaces
{
    public interface IRepository<T> where T : RepositoryEntity
    {
        bool Add(T entity);
        T Get(Guid id);
        List<T> GetAll();
        bool Exists(Guid id);
        bool Delete(Guid id);
        bool Update(T entity);
    }

}
