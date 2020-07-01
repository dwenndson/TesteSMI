using System.Collections.Generic;
using AutoMapper;
using IogServices.ExceptionHandlers.Exceptions;
using IogServices.Migrations;
using IogServices.Models.DTO;
using IogServices.Repositories;
using SmcUnregistered = IogServices.Models.DAO.SmcUnregistered;

namespace IogServices.Services.Impl
{
    public class SmcUnregisteredService : ISmcUnregisteredService
    {
        private readonly ISmcUnregisteredRespository _smcUnregisteredRespository;
        private readonly IMapper _mapper;
        public SmcUnregisteredService(ISmcUnregisteredRespository smcUnregisteredRespository, IMapper mapper)
        {
            _smcUnregisteredRespository = smcUnregisteredRespository;
            _mapper = mapper;
        }
        public List<SmcUnregisteredDto> GetAll()
        {
            return _mapper.Map<List<SmcUnregisteredDto>>(_smcUnregisteredRespository.GetAll());
        }

        public SmcUnregisteredDto Save(SmcUnregisteredDto t)
        {
            throw new System.NotImplementedException();
        }

        public SmcUnregisteredDto Update(SmcUnregisteredDto t)
        {
            var smc = GetExistingSmc(t.Serial);
            smc.UpdateFields(_mapper.Map<SmcUnregistered>(t));
            return _mapper.Map<SmcUnregisteredDto>(_smcUnregisteredRespository.Update(smc)) ;
        }
        
        public SmcUnregistered Update(SmcUnregistered t)
        {
            var smc = GetExistingSmc(t.Serial);
            smc.UpdateFields(_mapper.Map<SmcUnregistered>(t));
            return (_smcUnregisteredRespository.Update(smc)) ;
        }


        public SmcUnregistered Save(SmcUnregistered t)
        {
            return _smcUnregisteredRespository.GetBySerial(t.Serial) == null ? _smcUnregisteredRespository.Save(t) : Update(t);
        }

        public SmcUnregisteredDto FindBySerial(string serial)
        {
            return _mapper.Map<SmcUnregisteredDto>(_smcUnregisteredRespository.GetBySerial(serial));
        }

        public SmcUnregisteredDto Deactive(string serial)
        {
            var smc = GetExistingSmc(serial);
            smc.Active = false;
            return _mapper.Map<SmcUnregisteredDto>(_smcUnregisteredRespository.Update(smc));
        }

        public SmcUnregistered GetExistingSmc(string serial)
        {
            var smc = _smcUnregisteredRespository.GetBySerial(serial);
            if (smc == null)
                throw new InvalidConstraintException("O smc informado é inválido");
            return smc;
        }
    }
}