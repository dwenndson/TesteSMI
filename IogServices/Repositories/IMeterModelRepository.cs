using System;
using System.Collections.Generic;
using IogServices.Models.DAO;

namespace IogServices.Repositories
{
    public interface IMeterModelRepository : IGenericRepository<MeterModel>
    {
        MeterModel GetActiveByName(string name);
        MeterModel GetByName(string name);

    }
}