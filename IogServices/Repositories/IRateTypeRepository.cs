using System;
using System.Collections.Generic;
using IogServices.Models.DAO;

namespace IogServices.Repositories
{
    public interface IRateTypeRepository : IGenericRepository<RateType>
    {
        RateType GetByName(string name);
    }
}