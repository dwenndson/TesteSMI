using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories.Impl
{
    public class RateTypeRepository : IRateTypeRepository
    {
        private readonly IogContext _iogContext;

        public RateTypeRepository(IogContext iogContext)
        {
            this._iogContext = iogContext;
        }
        
        public List<RateType> GetAll()
        {
            List<RateType> rateTypes = this._iogContext.RateTypes.Where(rateType => rateType.Active)
                .OrderByDescending(rateType => rateType.UpdatedAt).ToList();
            return rateTypes;
        }

        public RateType GetById(Guid id)
        {
            RateType rateType = this._iogContext.RateTypes
                .FirstOrDefault(rateTypeIterator =>
                    rateTypeIterator.Id == id && rateTypeIterator.Active);
            return rateType;
        }

        public RateType Save(RateType rateType)
        {
            this._iogContext.RateTypes.Add(rateType);
            this._iogContext.SaveChanges();
            return rateType;
        }

        public RateType Update(RateType rateType)
        {
            this._iogContext.Update(rateType);
            this._iogContext.SaveChanges();
            return rateType;
        }

        public RateType GetByName(string name)
        {
            RateType rateType = this._iogContext.RateTypes
                .Include(rateTypeIterator => rateTypeIterator.Meters)
                .FirstOrDefault(rateTypeIterator =>
                    rateTypeIterator.Name == name && rateTypeIterator.Active);
            
            return rateType;
        }
    }
}