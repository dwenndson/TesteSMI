using System.Collections.Generic;
using AutoMapper;
using EletraSmcModels;
using IogServices.Models;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;
using NetworkObjects;

namespace IogServices.Services.Impl
{
    public class MeterAlarmService : IMeterAlarmService
    {
        private readonly IMeterAlarmRepository _meterAlarmRepository;
        private readonly IMapper _mapper;
        private readonly IMeterService _meterService;
        public MeterAlarmService(IMeterAlarmRepository meterAlarmRepository, IMapper mapper, IMeterService meterService)
        {
            _meterService = meterService;
            _meterAlarmRepository = meterAlarmRepository;
            _mapper = mapper;
        }

        public void ProcessMessage(AlarmMeterDto meter)
        {
            var alarm = _mapper.Map<MeterAlarmDto>(meter);
            foreach (var a in meter.Alarm)
            {
                alarm.Description = EnumHelperService<AlarmMeterTypes>.GetEnumDescription(a);
                Save(alarm);
            }
        }
        public List<MeterAlarmDto> GetAll()
        {
            return _mapper.Map<List<MeterAlarmDto>>(_meterAlarmRepository.GetAll());
        }

        public MeterAlarmDto Save(MeterAlarmDto t)
        {
            var alarmMeter = _mapper.Map<MeterAlarm>(t);
            alarmMeter.Meter = _meterService.GetExistingMeter(t.Serial);
            _meterAlarmRepository.Save(alarmMeter);
            return t;
        }

        public MeterAlarmDto Update(MeterAlarmDto t)
        {
            throw new System.NotImplementedException();
        }

        public List<MeterAlarmDto> GetBySerial(string serial)
        {
            return _mapper.Map<List<MeterAlarmDto>>(_meterAlarmRepository.GetBySerial(serial));
        }
    }
}