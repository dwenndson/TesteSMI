using System;
using System.Collections.Generic;
using IogServices.Models.DAO;
using IogServices.Models.DTO;

namespace IogServices.Services
{
    public interface ISmcModelService : IGenericService<SmcModelDto>
    {
        List<SmcModelDto> GetByManufacturer(string manufacturerName);
        SmcModelDto GetByName(string name);
        void Deactivate(string name);
        SmcModel GetExistingSmcModel(string name);
    }
}