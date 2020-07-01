using System;
using System.Collections.Generic;
using IogServices.Models.DAO;

namespace IogServices.Repositories
{
    public interface ISmcModelRepository : IGenericRepository<SmcModel>
    {
        SmcModel GetByName(string name);
    }
}