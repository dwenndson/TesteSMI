using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;

namespace IogServices.Repositories.Impl
{
    public class ModemRepository : IModemRepository
    {
        private readonly IogContext _iogContext;

        public ModemRepository(IogContext iogContext)
        {
            _iogContext = iogContext;
        }
        
        public List<Modem> GetAll()
        {
            throw new NotImplementedException();
        }

        public Modem GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Modem GetByEui(string eui)
        {
            return this._iogContext.Modems.LastOrDefault(modem => modem.DeviceEui == eui);
        }

        public Modem Save(Modem modem)
        {
            this._iogContext.Modems.Add(modem);
            this._iogContext.SaveChanges();
            return modem;
        }

        public Modem Update(Modem modem)
        {
            throw new NotImplementedException();
        }
    }
}