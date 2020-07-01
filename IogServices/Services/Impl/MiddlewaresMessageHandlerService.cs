using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EletraSmcModels;
using IogServices.HubConfig.PayloadHub;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Repositories;
using MqttClientLibrary.Models;
using NetworkObjects;
using NetworkObjects.Enum;
using Newtonsoft.Json;
using AlarmSmcType = IogServices.Enums.AlarmSmcType;
using ModemDto = IogServices.Models.DTO.ModemDto;
using RelayStatus = NetworkObjects.RelayStatus;
using SmcNotRegistered = NetworkObjects.SmcNotRegistered;

namespace IogServices.Services.Impl
{
    public class MiddlewaresMessageHandlerService : IMiddlewaresMessageHandlerService
    {
        private readonly IThreadService _threadService;
        private readonly IMeterEnergyService _meterEnergyService;
        private readonly IMeterService _meterService;
        private readonly ISmcService _smcService;
        private readonly IDeviceLogService _deviceLogService;
        private readonly ISmcAlarmService _smcAlarmService;
        private readonly IMeterAlarmService _meterAlarmService;
        private readonly ISmcUnregisteredService _smcUnregisteredService;
        private readonly IMeterUnregisteredService _meterUnregisteredService;

        private readonly IModemService _modemService;
        private readonly IMapper _mapper;
        private readonly IEventHubService _eventHubService;

        public MiddlewaresMessageHandlerService(IThreadService threadService, IMeterEnergyService meterEnergyService, 
            IMeterService meterService, IMapper mapper, IDeviceLogService deviceLogService, ISmcService smcService, 
            ISmcAlarmService smcAlarmService, IMeterAlarmService meterAlarmService, ISmcUnregisteredService smcUnregisteredService, IModemService modemService,
            IMeterUnregisteredService meterUnregisteredService, IEventHubService eventHubService)
        {
            _threadService = threadService;
            _meterEnergyService = meterEnergyService;
            _meterService = meterService;
            _deviceLogService = deviceLogService;
            _smcService = smcService;
            _smcAlarmService = smcAlarmService;
            _meterAlarmService = meterAlarmService;
            _smcUnregisteredService = smcUnregisteredService;
            _meterUnregisteredService = meterUnregisteredService;
            _modemService = modemService;
            _mapper = mapper;
            _eventHubService = eventHubService;
        }
        
        public void MessageReceivedFromMiddlewares(object sender, MessageReceivedArgs messageReceived)
        {
            var networkType = JsonConvert.DeserializeObject<BaseMessageDTO>(messageReceived.MessageReceived.Payload);
            switch (networkType.Type)
            {
                case NetworkMessageType.LOG_MESSAGE:
                    var logs = _mapper.Map<DeviceLog>(JsonConvert.DeserializeObject<LogMessage>(messageReceived.MessageReceived.Payload));
                    _deviceLogService.Save(logs);
                    _eventHubService.ALogWasGeneratedEvent(this, new PayloadUpdateLog(logs.Message, logs.DeviceSerial, logs.UpdatedAt.ToString(), logs.LogLevel));
                    break;
                case NetworkMessageType.CHANGE_MODEM_MESSAGE:
                    var modem = JsonConvert.DeserializeObject<ChangeModemMessageDto>(messageReceived.MessageReceived.Payload);
                    if (messageReceived.MessageReceived.Topic.Contains("smc"))
                        _smcService.SetModem(modem.NewSerial, modem.Identifier);
                    else
                    {
                        _meterService.SetModem(modem.NewSerial, modem.Identifier);
                    }
                    List<string> descript = modem.Identifier.Select(c => c.ToString()).ToList();
                    _eventHubService.AAlarmWasGeneratedEvent(this, new PayloadUpdateAlarm(descript, modem.NewSerial, DateTime.Now.ToString(), modem.Type));
                    break;
                case NetworkMessageType.ALARM_CPU:
                    var smcAlarm = JsonConvert.DeserializeObject<AlarmCpuDto>(messageReceived.MessageReceived.Payload);
                    _smcAlarmService.Save(_mapper.Map<SmcAlarmDto>(smcAlarm));
                    //Init websocket
                    List<string> description = smcAlarm.Alarm.Select(c => c.ToString()).ToList();
                    _eventHubService.AAlarmWasGeneratedEvent(this, new PayloadUpdateAlarm (description, smcAlarm.Serial, smcAlarm.ReadTime.ToString(), smcAlarm.Type));
                    //End websocket
                    break;
                case NetworkMessageType.ALARM_METER:
                    var meterAlarm =
                        JsonConvert.DeserializeObject<AlarmMeterDto>(messageReceived.MessageReceived.Payload);
                   _meterAlarmService.ProcessMessage(meterAlarm);
                    //Init websocket
                    _eventHubService.AAlarmWasGeneratedEvent(this, new PayloadUpdateAlarm(meterAlarm.Alarm, meterAlarm.Serial, meterAlarm.ReadTime.ToString(), meterAlarm.Type));
                    //End websocket
                    break;
                case NetworkMessageType.MEASURES:
                    var measure = JsonConvert.DeserializeObject<ActiveEnergyDto>(messageReceived.MessageReceived.Payload);
                    _meterEnergyService.Save(_mapper.Map<MeterEnergyDto>(measure));
                    _eventHubService.AAlarmWasGeneratedEvent(this, new PayloadUpdateAlarm(null, measure.Serial, measure.ReadingTime, measure.Type));
                    break;
                case NetworkMessageType.SMC_NOT_REGISTERED:
                    var smc = JsonConvert.DeserializeObject<SmcNotRegistered>(messageReceived.MessageReceived.Payload);
                    _smcUnregisteredService.Save(_mapper.Map<SmcUnregistered>(smc));
                    break;
                case NetworkMessageType.SMI_NOT_REGISTERED:
                    var meter = JsonConvert.DeserializeObject<MeterNotregistered>(messageReceived.MessageReceived.Payload);
                    _meterUnregisteredService.Save(_mapper.Map<MeterUnregistered>(meter));
                    break;
                case NetworkMessageType.SMC_COMISSIONED:
                    var smcComissioned = JsonConvert.DeserializeObject<SmcComissioned>(messageReceived.MessageReceived.Payload);
                    _smcService.ChangeComissionedStatus(smcComissioned.Serial, smcComissioned.Status);
                    break;
                case NetworkMessageType.SMI_COMISSIONED:
                    var meterComissionrd = JsonConvert.DeserializeObject<SmiComissioned>(messageReceived.MessageReceived.Payload);
                    _meterService.ChangeComissionedStatus(meterComissionrd.Serial, meterComissionrd.Status);
                    break;
                case  NetworkMessageType.UPDATE_MODEM:
                    var updatedModem = JsonConvert.DeserializeObject<UpdateModemDto>(messageReceived.MessageReceived.Payload);
                    _modemService.Update(_mapper.Map<ModemDto>(updatedModem));
                    break;
                case NetworkMessageType.RELAY_STATUS:
                    var relayStatus = JsonConvert.DeserializeObject<RelayStatus>(messageReceived.MessageReceived.Payload);
                    _meterService.SetRelayStatus(relayStatus.Serial, relayStatus.AccountantStatus);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}