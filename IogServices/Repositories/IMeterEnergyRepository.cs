using System;
using System.Collections.Generic;
using IogServices.Models.DAO;
using IogServices.Models.DTO;

namespace IogServices.Repositories
{
    public interface IMeterEnergyRepository : IGenericRepository<MeterEnergy>
    {
        List<MeterEnergy> GetAllBySerial(string serial);
    }
}