using System;
using System.Collections.Generic;
using AutoMapper;
using IogServices.ExceptionHandlers.Exceptions;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;
using IogServices.Util;

namespace IogServices.Services.Impl
{
    public class SmcService : ISmcService
    {
        private readonly ISmcRepository _smcRepository;
        private readonly ISmcModelService _smcModelService;
        private readonly IModemRepository _modemRepository;
        private readonly IMeterRepository _meterRepository;
        private readonly IServicesUtils _servicesUtils;
        private readonly IMapper _mapper;

        public SmcService(ISmcRepository smcRepository, ISmcModelService smcModelService,
            IModemRepository modemRepository, IMeterRepository meterRepository, IServicesUtils servicesUtils,
            IMapper mapper)
        {
            _smcRepository = smcRepository;
            _smcModelService = smcModelService;
            _modemRepository = modemRepository;
            _meterRepository = meterRepository;
            _servicesUtils = servicesUtils;
            _mapper = mapper;
        }

        public List<SmcDto> GetAll()
        {
            return _mapper.Map<List<Smc>, List<SmcDto>>(_smcRepository.GetAll());
        }

        public SmcDto Save(SmcDto smcDto)
        {
            var savedSmc = _smcRepository.GetBySerial(smcDto.Serial);
            var smc = _mapper.Map<SmcDto, Smc>(smcDto);
            smc.SmcModel = _smcModelService.GetExistingSmcModel(smc.SmcModel.Name);
            if (savedSmc == null)
            
                return _mapper.Map<Smc, SmcDto>(_smcRepository.Save(smc));
            
            if (savedSmc.Active)
                throw new ExistentEntityException("O smc de serial " + savedSmc.Serial + " já existe");
            
            savedSmc.UpdateFields(smc);
            savedSmc.Active = true;
            return _mapper.Map<Smc, SmcDto>(_smcRepository.Update(savedSmc));
        }

        public SmcDto Update(SmcDto smcDto)
        {
            var smc = _mapper.Map<SmcDto, Smc>(smcDto);
            var savedSmc = GetExistingSmc(smcDto.Serial);
            var smcModel = _smcModelService.GetExistingSmcModel(smcDto.SmcModel.Name);
            smc.SmcModel = smcModel;
            savedSmc.UpdateFields(smc);
            return _mapper
                .Map<Smc, SmcDto>(
                    _smcRepository.Update(savedSmc)
                );
        }

        public SmcDto GetBySerial(string serial)
        {
            return _mapper.Map<Smc, SmcDto>(_smcRepository.GetBySerial(serial));
        }

        public void SetModem(string serial, string eui)
        {
            var smc = _smcRepository.GetBySerial(serial);
            if (smc == null) return;
            var modem = _modemRepository.GetByEui(eui);
            if (modem == null)
            {
                modem = new Modem {DeviceEui = eui};
                modem = _modemRepository.Save(modem);
            }
            else
                _servicesUtils.RemoveModemOfAllEntities(modem);
            
            smc.Modem = modem;
            smc.Meters.ForEach(meter => { meter.Modem = modem; _meterRepository.Update(meter);});
            _smcRepository.Update(smc);
        }

        public void Deactivate(string serial)
        {
            var smc = GetExistingSmc(serial);
            smc.Meters.ForEach(meter =>
            {
                meter.Active = false;
                meter.Modem = null;
                _meterRepository.Update(meter);
            });
            
            smc.Modem = null;
            smc.Active = false;
            _smcRepository.Update(smc);
        }
        
        public Smc GetExistingSmc(string serial)
        {
            var savedSmc = _smcRepository.GetBySerial(serial);
            if (savedSmc == null)
                throw new InvalidConstraintException("O smc informado é inválido");
            return savedSmc;
        }

        public Smc GetSmc(string serial)
        {
            return serial == null ? null : _smcRepository.GetBySerial(serial);
        }

        public List<MeterDto> GetMeterBySmc(string serial)
        {
            return _mapper.Map<List<Meter>, List<MeterDto>>(_meterRepository.GetAllBySmc(serial));

        }

        public List<SmcDto> GetAllComissioned()
        {
            return _mapper.Map<List<Smc>, List<SmcDto>>(_smcRepository.GetAllComissioned());
        }

        public List<SmcDto> GetAllNotComissioned()
        {
            return _mapper.Map<List<Smc>, List<SmcDto>>(_smcRepository.GetAllNotComissioned());
        }

        public void ChangeComissionedStatus(string serial, bool status)
        {
            var smc = GetExistingSmc(serial);
            smc.Comissioned = status;
            
            foreach (var meter in smc.Meters)
                meter.Comissioned = status;
            _smcRepository.Update(smc);
        }
    }
}