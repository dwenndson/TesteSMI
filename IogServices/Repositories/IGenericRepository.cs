using System;
using System.Collections.Generic;

namespace IogServices.Repositories
{
    public interface IGenericRepository<T>
    {
        List<T> GetAll();
        T GetById(Guid id);
        T Save(T t);
        T Update(T t);
    }
}