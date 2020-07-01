using System;
using System.Collections.Generic;
using IogServices.Models.DAO;
using IogServices.Models.DTO;

namespace IogServices.Services
{
    public interface IRateTypeService : IGenericService<RateTypeDto>
    {
        RateTypeDto GetByName(string name);
        void Deactivate(string name);
        RateType GetExistingRateType(string name);
    }
}