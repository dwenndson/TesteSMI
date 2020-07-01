using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using IogServices.ExceptionHandlers.Exceptions;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;
using IogServices.Util;
using Microsoft.EntityFrameworkCore.Metadata;
using NetworkObjects.Enum;
using Newtonsoft.Json;

namespace IogServices.Services.Impl
{
    public class MeterService : IMeterService
    {
        private readonly IMeterRepository _meterRepository;
        private readonly IModemRepository _modemRepository;
        private readonly IMeterModelService _meterModelService;
        private readonly IRateTypeService _rateTypeService;
        private readonly ISmcService _smcService;
        private readonly IMapper _mapper;
        private readonly IServicesUtils _servicesUtils;
        private readonly IMeterKeyService _meterKeyService;
        private readonly ISmcForwarderService _smcForwarderService;
        private readonly IMeterForwarderService _meterForwarderService;
        private readonly ISmcModelService _smcModelService;
        private readonly ISmcUnregisteredService _smcUnregisteredService;
        private readonly IMeterUnregisteredService _meterUnregisteredService;
        private readonly ITicketService _ticketService;
        private readonly IHubService _hubService;

        public MeterService(IMeterRepository meterRepository, IModemRepository modemRepository,
            IMeterModelService meterModelService,
            IRateTypeService rateTypeService, ISmcService smcService, IMapper mapper, 
            IServicesUtils servicesUtils, IMeterKeyService meterKeyService, ISmcForwarderService smcForwarderService,
            ISmcUnregisteredService smcUnregisteredService,
            IMeterForwarderService meterForwarderService, ITicketService ticketService,
            ISmcModelService smcModelService, IMeterUnregisteredService meterUnregisteredService,
            IHubService hubService)
        {
            _meterRepository = meterRepository;
            _modemRepository = modemRepository;
            _meterModelService = meterModelService;
            _rateTypeService = rateTypeService;
            _smcService = smcService;
            _mapper = mapper;
            _servicesUtils = servicesUtils;
            _meterKeyService = meterKeyService;
            _smcForwarderService = smcForwarderService;
            _meterForwarderService = meterForwarderService;
            _smcModelService = smcModelService;
            _hubService = hubService;
            _smcUnregisteredService = smcUnregisteredService;
            _meterUnregisteredService = meterUnregisteredService;
            _ticketService = ticketService;
        }

        public List<MeterDto> GetAll()
        {
            var meters = _mapper.Map<List<Meter>, List<MeterDto>>(_meterRepository.GetAll());
            if (meters == null)
                return new List<MeterDto>();
                
            foreach (var meter in meters)
                _ticketService.CheckCommandSituation(meter);
            
            return meters;
        }

        public MeterDto GetBySerial(string serial)
        {
            var meter = _mapper.Map<Meter, MeterDto>(_meterRepository.GetActiveBySerial(serial));
            if (meter != null)
                _ticketService.CheckCommandSituation(meter);

            return meter;
        }

        public MeterDto Save(MeterDto meterDto)
        {
            return meterDto.Smc is null ? SaveSmi(meterDto) : SaveSmc(meterDto);
        }

        private MeterDto SaveSmi (MeterDto meterDto)
        {
            var savedMeter = _meterRepository.GetBySerial(meterDto.Serial);
            var meter = _mapper.Map<MeterDto, Meter>(meterDto);
           
            if (savedMeter != null)
                return CheckMeterRegistered(meter, meterDto);

            meter.MeterModel = _meterModelService.GetExistingMeterModel(meterDto.MeterModel.Name);
            meter.RateType = _rateTypeService.GetExistingRateType(meterDto.RateType.Name);
            meter.MeterKeys = _meterKeyService.GetBySerial(meterDto.Serial);
            meter.AccountantStatus = AccountantStatus.NO_INFORMATION;
            var status = _meterForwarderService.ForwardCreation(_mapper.Map<Meter, MeterDto>(meter));
            if (!status.IsSuccessStatusCode)
                throw new BadRequestException(JsonConvert
                    .DeserializeObject<ErrorMessageDto>(status.Content.ReadAsStringAsync().Result).ErrorMessage);
            var meterUnregisteredDto = _meterUnregisteredService.FindBySerial(meter.Serial);
            if (meterUnregisteredDto != null)
                _meterUnregisteredService.Deactive(meter.Serial);
            return _mapper.Map<Meter, MeterDto>(_meterRepository.Save(meter));
        }
        
        private MeterDto SaveSmc (MeterDto meterDto)
        {
            var savedMeter = _meterRepository.GetBySerial(meterDto.Serial);
            var meter = _mapper.Map<MeterDto, Meter>(meterDto);
            var smc = _smcService.GetSmc(meterDto.Smc.Serial) ?? CreateSmc(meterDto.Smc, 
                           _mapper.Map<MeterKeys, KeysDto>(_meterKeyService.GetBySerial(meterDto.Smc.Serial)));

            return savedMeter != null ? CheckSmcMeterRegistered(savedMeter, meterDto, smc) : CreateMeter(meter, meterDto, smc);
        }

        private Smc CreateSmc(SmcDto smc, KeysDto keysDto)
        {
            smc.KeysDto = keysDto;
            smc.SmcModel = _smcModelService.GetByName(smc.SmcModel.Name);
            var status = _smcForwarderService.ForwardCreation(smc);
            if (!status.IsSuccessStatusCode)
                throw new BadRequestException(JsonConvert
                    .DeserializeObject<ErrorMessageDto>(status.Content.ReadAsStringAsync().Result).ErrorMessage);
            _smcService.Save(smc);
            return  _smcService.GetExistingSmc(smc.Serial);
        }

        private MeterDto CreateMeter(Meter meter, MeterDto meterDto, Smc smc)
        {
            meter.UpdateFields(meter, 
                _meterModelService.GetExistingMeterModel(meterDto.MeterModel.Name), 
                _rateTypeService.GetExistingRateType(meterDto.RateType.Name), smc, _meterKeyService.GetBySerial(smc.Serial)); 
            CheckSmcPositions(meter);
            var status = _meterForwarderService.ForwardCreation(_mapper.Map<Meter, MeterDto>(meter));
            if (!status.IsSuccessStatusCode)
                throw new BadRequestException(JsonConvert
                    .DeserializeObject<ErrorMessageDto>(status.Content.ReadAsStringAsync().Result).ErrorMessage);
            var smcUnregisteredDto = _smcUnregisteredService.FindBySerial(smc.Serial);
            if (smcUnregisteredDto != null)
                _smcUnregisteredService.Deactive(smc.Serial);
            meter.AccountantStatus = AccountantStatus.NO_INFORMATION;
            return _mapper.Map<Meter, MeterDto>(_meterRepository.Save(meter));
        }
        private MeterDto CheckSmcMeterRegistered(Meter meter, MeterDto meterDto, Smc smc)
        {
            if (meter.Active)
                throw new ExistentEntityException("O medidor de serial " + meter.Serial + " já existe");
                
            meter.UpdateFields(meter, 
                _meterModelService.GetExistingMeterModel(meter.MeterModel.Name), 
                _rateTypeService.GetExistingRateType(meterDto.RateType.Name), smc,_meterKeyService.GetBySerial(smc.Serial) );
            CheckSmcPositions(meter);
            meter.Active = true;
            var status = _meterForwarderService.ForwardEdition(_mapper.Map<Meter, MeterDto>(meter));
            if (!status.IsSuccessStatusCode)
                throw new BadRequestException(JsonConvert
                    .DeserializeObject<ErrorMessageDto>(status.Content.ReadAsStringAsync().Result).ErrorMessage);
            return _mapper.Map<Meter, MeterDto>(
                _meterRepository.Update(meter));
        }

        private MeterDto CheckMeterRegistered(Meter meter, MeterDto meterDto)
        {
            if (meter.Active)
                throw new ExistentEntityException("O medidor de serial " + meter.Serial + " já existe"); 
            meter.UpdateFields(meter, 
                _meterModelService.GetExistingMeterModel(meterDto.MeterModel.Name), 
                _rateTypeService.GetExistingRateType(meterDto.RateType.Name), 
                _meterKeyService.GetBySerial(meterDto.Serial));
            meter.Active = true;
            return _mapper.Map<Meter, MeterDto>(
                _meterRepository.Update(meter));
        }
        public MeterDto Update(MeterDto meterDto)
        {
            var meter = _mapper.Map<MeterDto, Meter>(meterDto);
            var savedMeter = GetExistingMeter(meterDto.Serial);
            if (savedMeter.Smc == null)
            
                savedMeter.UpdateFields(meter, 
                    _meterModelService.GetExistingMeterModel(meterDto.MeterModel.Name), 
                    _rateTypeService.GetExistingRateType(meterDto.RateType.Name),_meterKeyService.GetBySerial(meterDto.Serial)  );
            else
            {
                var savedSmc = _smcService.GetSmc(savedMeter.Smc.Serial);
                savedMeter.UpdateFields(meter, 
                    _meterModelService.GetExistingMeterModel(meterDto.MeterModel.Name), 
                    _rateTypeService.GetExistingRateType(meterDto.RateType.Name), savedSmc,_meterKeyService.GetBySerial(meterDto.Serial)  );
            }
            
            
            var status = _meterForwarderService.ForwardEdition(_mapper.Map<Meter, MeterDto>(savedMeter));
            if (!status.IsSuccessStatusCode)
                throw new BadRequestException(JsonConvert
                    .DeserializeObject<ErrorMessageDto>(status.Content.ReadAsStringAsync().Result).ErrorMessage);
            return _mapper
                .Map<Meter, MeterDto>(
                    _meterRepository.Update(savedMeter)
                );
        }

        public void Deactivate(string serial)
        {
            var meter = GetExistingMeter(serial);
            meter.Modem = null;
            meter.Active = false;
            var status = _meterForwarderService.ForwardDeletion(_mapper.Map<Meter, MeterDto>(meter));
            if (!status.IsSuccessStatusCode)
                throw new BadRequestException(JsonConvert
                    .DeserializeObject<ErrorMessageDto>(status.Content.ReadAsStringAsync().Result).ErrorMessage);
            _meterRepository.Update(meter);
        }

        public void SetModem(string serial, string eui)
        {
            var meter = _meterRepository.GetActiveBySerial(serial);
            if (meter == null) return;            
            var modem = _modemRepository.GetByEui(eui);
            if (modem == null)
            {
                modem = new Modem {DeviceEui = eui};
                modem = _modemRepository.Save(modem);
            }
            else
                _servicesUtils.RemoveModemOfAllEntities(modem);
            
            meter.Modem = modem;
            _meterRepository.Update(meter);
        }

        public Meter GetExistingMeter(string serial)
        {
            var savedMeter = _meterRepository.GetActiveBySerial(serial);
            if (savedMeter == null)
                throw new InvalidConstraintException("O medidor informado é inválido");
            return savedMeter;
        }

        public List<MeterDto> GetMetersBySmc(string serial)
        {
            return _mapper.Map<List<Meter>, List<MeterDto>>(_meterRepository.GetAllBySmc(serial));
        }

        public List<MeterDto> GetMetersComissioned()
        {
            var meters = _mapper.Map<List<Meter>, List<MeterDto>>(_meterRepository.GetAllComissioned());
            if (meters.Count == 0) return meters;
            foreach (var meter in meters)
                _ticketService.CheckCommandSituation(meter);
            return meters;
        }

        public List<MeterDto> GetMetersNotComissioned()
        {
            return _mapper.Map<List<Meter>, List<MeterDto>>(_meterRepository.GetAllNotComissioned());
        }

        public void ChangeComissionedStatus(string serial, bool status)
        {
            var meter = GetExistingMeter(serial);
            meter.Comissioned = status;
            _meterRepository.Update(meter);
        }

        public List<MeterDto> GetAllNotBelongingToASmc()
        {
            return _mapper.Map<List<Meter>, List<MeterDto>>(_meterRepository.GetAllNotBelongingToASmc());
        }

        public void SetRelayStatus(string serial, AccountantStatus accountantStatus)
        {
            var meter = GetExistingMeter(serial);
            meter.AccountantStatus = accountantStatus;
            _meterRepository.Update(meter);
        }

        public Meter GetMeter(string serial)
        {
            return _meterRepository.GetActiveBySerial(serial);
        }

        private void CheckSmcPositions(Meter meter)
        {
            if (meter.Smc.Meters == null) return;
            if ((int) meter.Phase +  meter.Smc.Meters.Sum(m => (int) m.Phase) > meter.Smc.SmcModel.PositionsCount)
                throw new BadRequestException("O SMC não possui espaço disponível para o medidor informado");
        }
    }
}