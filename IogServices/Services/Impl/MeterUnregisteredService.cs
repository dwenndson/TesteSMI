using System.Collections.Generic;
using AutoMapper;
using IogServices.ExceptionHandlers.Exceptions;
using IogServices.Migrations;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;

namespace IogServices.Services.Impl
{
    public class MeterUnregisteredService : IMeterUnregisteredService
    {
        private readonly IMeterUnregisteredRespository _meterUnregisteredRespository;
        private readonly IMapper _mapper;
        public MeterUnregisteredService(IMeterUnregisteredRespository meterUnregisteredRespository, IMapper mapper)
        {
            _meterUnregisteredRespository = meterUnregisteredRespository;
            _mapper = mapper;
        }
        public List<MeterUnregisteredDto> GetAll()
        {
            return _mapper.Map<List<MeterUnregisteredDto>>(_meterUnregisteredRespository.GetAll());
        }

        public MeterUnregisteredDto Save(MeterUnregisteredDto t)
        {
            throw new System.NotImplementedException();
        }

        public MeterUnregisteredDto Update(MeterUnregisteredDto t)
        {
            var unregistered = GetExistingSmi(t.Serial);
            unregistered.UpdateFields(_mapper.Map<MeterUnregistered>(t));
            return _mapper.Map<MeterUnregisteredDto>(_meterUnregisteredRespository.Update(unregistered)) ;
        }
        
        public MeterUnregistered Update(MeterUnregistered t)
        {
            var meterUnregistered = GetExistingSmi(t.Serial);
            meterUnregistered.UpdateFields(_mapper.Map<MeterUnregistered>(t));
            return (_meterUnregisteredRespository.Update(meterUnregistered)) ;
        }


        public MeterUnregistered Save(MeterUnregistered t)
        {
            return _meterUnregisteredRespository.GetBySerial(t.Serial) == null ? _meterUnregisteredRespository.Save(t) : Update(t);
        }

        public MeterUnregisteredDto FindBySerial(string serial)
        {
            return _mapper.Map<MeterUnregisteredDto>(_meterUnregisteredRespository.GetBySerial(serial));
        }

        public MeterUnregisteredDto Deactive(string serial)
        {
            var meterUnregistered = GetExistingSmi(serial);
            meterUnregistered.Active = false;
            return _mapper.Map<MeterUnregisteredDto>(_meterUnregisteredRespository.Update(meterUnregistered));
        }

        public MeterUnregistered GetExistingSmi(string serial)
        {
            var unregistered = _meterUnregisteredRespository.GetBySerial(serial);
            if (unregistered == null)
                throw new InvalidConstraintException("O medidor informado é inválido");
            return unregistered;
        }
    }
}