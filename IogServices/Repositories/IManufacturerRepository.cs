using System;
using System.Collections.Generic;
using IogServices.Models.DAO;

namespace IogServices.Repositories
{
    public interface IManufacturerRepository : IGenericRepository<Manufacturer>
    {
        Manufacturer GetActiveByName(string name);
        Manufacturer GetByName(string name);
    }
}