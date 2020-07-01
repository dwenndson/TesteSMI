using System;
using System.Collections.Generic;
using IogServices.Models.DAO;
using IogServices.Models.DTO;

namespace IogServices.Services
{
    public interface IMeterModelService : IGenericService<MeterModelDto>
    {
        List<MeterModelDto> GetByManufacturer(string manufacturerName);
        MeterModelDto GetByName(string name);
        void Deactivate(string name);
        MeterModel GetExistingMeterModel(string name);
    }
}