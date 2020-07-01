using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;
using Microsoft.EntityFrameworkCore;

namespace IogServices.Repositories.Impl
{
    public class SmcModelRepository : ISmcModelRepository
    {
        private readonly IogContext _iogContext;

        public SmcModelRepository(IogContext iogContext)
        {
            this._iogContext = iogContext;
        }
        
        public List<SmcModel> GetAll()
        {
            List<SmcModel> smcModels = this._iogContext.SmcModels
                .Include(smcModel => smcModel.Manufacturer).Where(smcModel => smcModel.Active)
                .OrderByDescending(smcModel => smcModel.UpdatedAt).ToList();
            return smcModels;
        }

        public SmcModel GetByName(string name)
        {
            SmcModel smcModel = this._iogContext.SmcModels
                .Include(smcModelIterator => smcModelIterator.Manufacturer)
                .Include(smcModelIterator => smcModelIterator.Smcs)
                .FirstOrDefault(smcModelIterator =>
                    smcModelIterator.Name == name && smcModelIterator.Active);

            if (smcModel != null)
            {
                smcModel.Smcs = smcModel.Smcs.Where(smc => smc.Active).ToList();
            }
            
            return smcModel;
        }
        public SmcModel GetById(Guid id)
        {
            SmcModel smcModel = this._iogContext.SmcModels
                .Include(smcModelIterator => smcModelIterator.Manufacturer)
                .FirstOrDefault(smcModelIterator =>
                    smcModelIterator.Id == id && smcModelIterator.Active);
            return smcModel;
        }

        public SmcModel Save(SmcModel smcModel)
        {
            this._iogContext.SmcModels.Add(smcModel);
            this._iogContext.SaveChanges();
            return smcModel;
        }

        public SmcModel Update(SmcModel smcModel)
        {
            this._iogContext.Update(smcModel);
            this._iogContext.SaveChanges();
            return smcModel;
        }
    }
}