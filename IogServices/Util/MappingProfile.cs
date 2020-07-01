using System.Collections.Generic;
using AutoMapper;
using DocumentFormat.OpenXml.Office2013.Drawing;
using IogServices.Enums;
using IogServices.Models;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using IogServices.Services.Impl;
using NetworkObjects;
using CommandTicketDto = IogServices.Models.DTO.CommandTicketDto;
using SmcNotRegistered = NetworkObjects.SmcNotRegistered;

namespace IogServices.Util
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<KeysDto, MeterKeys>();
            CreateMap<MeterKeys, KeysDto>();

            CreateMap<Manufacturer, ManufacturerDto>();
            CreateMap<ManufacturerDto, Manufacturer>();

            CreateMap<SmcModel, SmcModelDto>();
            CreateMap<SmcModelDto, SmcModel>();

            CreateMap<MeterModel, MeterModelDto>();
            CreateMap<MeterModelDto, MeterModel>();

            CreateMap<RateType, RateTypeDto>();
            CreateMap<RateTypeDto, RateType>();

            CreateMap<Meter, MeterDto>().ForMember(s => s.KeysDto, opt => opt.MapFrom(o => o.MeterKeys));
            CreateMap<MeterDto, Meter>();

            CreateMap<MeterEnergy, MeterEnergyDto>();
            CreateMap<MeterEnergyDto, MeterEnergy>();

            CreateMap<UpdateModemDto, ModemDto>();
            CreateMap<Modem, ModemDto>();
            CreateMap<ModemDto, Modem>();

            CreateMap<Smc, SmcDto>();
            CreateMap<SmcDto, Smc>();

            CreateMap<User, UserDto>()
                .ForMember(user => user.Password, 
                    opt => opt.Ignore());
            CreateMap<UserDto, User>();


            CreateMap<SmcDto, EletraSmcModels.SmcDto>()
                .ForMember(x => x.AK, opt => opt.MapFrom(o => o.KeysDto.Ak))
                .ForMember(x => x.EK, opt => opt.MapFrom(o => o.KeysDto.Ek))
                .ForMember(x => x.MK, opt => opt.MapFrom(o => o.KeysDto.Mk));
            CreateMap<MeterDto, EletraSmcModels.MeterDto>()
                .ForMember(x=> x.UCCode, opt => opt.MapFrom(o => o.Installation))
                .ForMember(x=> x.MeterPhase, opt => opt.MapFrom(o => o.Phase));

            CreateMap<MeterDto, EletraSmiModels.MeterDto>()
                .ForMember(x => x.AK, opt => opt.MapFrom(o => o.KeysDto.Ak))
                .ForMember(x => x.EK, opt => opt.MapFrom(o => o.KeysDto.Ek))
                .ForMember(x => x.MK, opt => opt.MapFrom(o => o.KeysDto.Mk))
                .ForMember(x=> x.MeterPhase, opt => opt.MapFrom(o => o.Phase));
            
            CreateMap<ModemDto, EletraSmiModels.ModemDto>();
            CreateMap<ModemDto, EletraSmcModels.ModemDto>();
            CreateMap<CommandTicket, CommandTicketDto>();
            CreateMap<CommandTicketDto, CommandTicket>();
            
            CreateMap<Ticket, TicketDto>();
            CreateMap<TicketDto, Ticket>();
            
            CreateMap<SmcAlarm, SmcAlarmDto>();
            CreateMap<SmcAlarmDto, SmcAlarm>();

            CreateMap<MeterAlarm, MeterAlarmDto>();
            CreateMap<MeterAlarmDto, MeterAlarm>();
            
            CreateMap<AlarmMeterDto, MeterAlarmDto>()
                .ForMember(x => x.ReadDateTime, opt => opt.MapFrom(o => o.ReadTime));

            
            CreateMap<AlarmCpuDto, SmcAlarmDto>()
                .ForMember(x => x.Description,
                    opt => opt.MapFrom(o => EnumHelperService<AlarmSmcType>.GetEnumDescription(o.Alarm)))
                .ForMember(x => x.ReadDateTime, opt => opt.MapFrom(o => o.ReadTime));

            CreateMap<ActiveEnergyDto, MeterEnergyDto>()
                .ForMember(x => x.DirectEnergy, opt => opt.MapFrom(o => o.Positive))
                .ForMember(x => x.ReverseEnergy, opt => opt.MapFrom(o => o.Negative))
                .ForPath(x => x.Meter.Serial, opt => opt.MapFrom(o => o.Serial));

            CreateMap<LogMessage, DeviceLog>()
                .ForMember(x=> x.LogLevel, opt => opt.MapFrom(o => o.Level));
            CreateMap<DeviceLog, DeviceLogDto>();
            CreateMap<DeviceLogDto, DeviceLog>();

            CreateMap<MeterNotregistered, MeterUnregistered>();
            CreateMap<MeterUnregistered, MeterUnregisteredDto>();

            CreateMap<SmcNotRegistered, SmcUnregistered>();
            CreateMap<SmcUnregistered, SmcUnregisteredDto>();


        }
    }
}