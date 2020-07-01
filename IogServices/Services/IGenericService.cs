using System.Collections.Generic;

namespace IogServices.Services
{
    public interface IGenericService<T>
    {
        List<T> GetAll();
        T Save(T t);
        T Update(T t);
    }
}