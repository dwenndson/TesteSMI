using System;
using System.Collections.Generic;
using AutoMapper;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;

namespace IogServices.Services.Impl
{
    public class SmcAlarmService : ISmcAlarmService
    {
        private readonly ISmcAlarmRepository _alarmSmcAlarmRepository;
        private readonly ISmcService _smcService;
        private readonly IMapper _mapper;


        public SmcAlarmService(ISmcAlarmRepository smcAlarmRepository,  ISmcService smcService, IMapper mapper)
        {
            _alarmSmcAlarmRepository = smcAlarmRepository;
            _smcService = smcService;
            _mapper = mapper;
        }
        public List<SmcAlarmDto> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public SmcAlarmDto Save(SmcAlarmDto t)
        {
            var alarmSmc = _mapper.Map<SmcAlarm>(t);
            alarmSmc.Smc =  _smcService.GetExistingSmc(t.Serial);
            _alarmSmcAlarmRepository.Save(alarmSmc);
            return t;
        }

        public SmcAlarmDto Update(SmcAlarmDto t)
        {
            throw new System.NotImplementedException();
        }

        public List<SmcAlarmDto> GetBySerial(string serial)
        {
            return _mapper.Map<List<SmcAlarmDto>>(_alarmSmcAlarmRepository.GetBySerial(serial));
        }
    }
}