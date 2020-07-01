using System;
using System.Collections.Generic;
using AutoMapper;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;

namespace IogServices.Services.Impl
{
    public class MeterEnergyService : IMeterEnergyService
    {
        private readonly IMapper _mapper;
        private readonly IMeterEnergyRepository _meterEnergyRepository;
        private readonly IMeterService _meterService;

        public MeterEnergyService(IMapper mapper, IMeterEnergyRepository meterEnergyRepository, IMeterService meterService)
        {
            _mapper = mapper;
            _meterEnergyRepository = meterEnergyRepository;
            _meterService = meterService;
        }

        public List<MeterEnergyDto> GetAllBySerial(string serial)
        {
            return _mapper.Map<List<MeterEnergy>, List<MeterEnergyDto>>(
                _meterEnergyRepository.GetAllBySerial(serial));
        }

        public List<MeterEnergyDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public MeterEnergyDto Save(MeterEnergyDto meterEnergyDto)
        {
            var meter = _meterService.GetExistingMeter(meterEnergyDto.Meter.Serial);
            var newMeterEnergy = _mapper.Map<MeterEnergyDto, MeterEnergy>(meterEnergyDto);
            newMeterEnergy.Meter = meter;
            return _mapper.Map<MeterEnergy, MeterEnergyDto>(
                _meterEnergyRepository.Save(newMeterEnergy));
        }

        public MeterEnergyDto Update(MeterEnergyDto t)
        {
            throw new NotImplementedException();
        }

        public MeterEnergyDto GetById(Guid id)
        {
            return _mapper.Map<MeterEnergy, MeterEnergyDto>(_meterEnergyRepository.GetById(id));
        }
    }
}