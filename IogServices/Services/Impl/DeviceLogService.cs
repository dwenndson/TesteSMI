using AutoMapper;
using IogServices.HubConfig.PayloadHub;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace IogServices.Services.Impl
{
    public class DeviceLogService: IDeviceLogService
    {
        private readonly IDeviceLogRepository _deviceLogRepository;
        private readonly IMapper _mapper;
        private readonly IEventHubService _eventHubService;

        public DeviceLogService(IDeviceLogRepository deviceLogRepository, IMapper mapper, IEventHubService eventHubService)
        {
            _deviceLogRepository = deviceLogRepository;
            _mapper = mapper;
            _eventHubService = eventHubService;
        }
        public List<DeviceLogDto> GetBySerial(string serial)
        {
            return _mapper.Map<List<DeviceLogDto>>(_deviceLogRepository.GetBySerial(serial)); ;
        }

        public List<DeviceLogDto> GetBySerialFilterByLogLevel(string serial, LogLevel level)
        {
            return _mapper.Map<List<DeviceLogDto>>( _deviceLogRepository.GetBySerialFilterByLogLevel(serial, level));
        }

        public DeviceLog Save(DeviceLog deviceLog)
        {
            var updatedAt = Convert.ToString(deviceLog.UpdatedAt);
            _eventHubService.ALogWasGeneratedEvent(this, new PayloadUpdateLog(deviceLog.Message, deviceLog.DeviceSerial, updatedAt, deviceLog.LogLevel));
            return _deviceLogRepository.Save(deviceLog);
        }
    }
}