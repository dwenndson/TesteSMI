using System;
using System.Collections.Generic;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Services.Impl;

namespace IogServices.Services
{
    public interface IMeterEnergyService : IGenericService<MeterEnergyDto>
    {
        List<MeterEnergyDto> GetAllBySerial(string serial);
        MeterEnergyDto GetById(Guid id);
    }
}