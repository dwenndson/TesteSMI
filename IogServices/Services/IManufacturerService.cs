using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;

namespace IogServices.Services
{
    public interface IManufacturerService : IGenericService<ManufacturerDto>
    {
        ManufacturerDto GetByName(string name);
        void Deactivate(string name);
        Manufacturer GetExistingManufacturer(string name);
    }
}