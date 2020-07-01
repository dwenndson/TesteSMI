using AutoMapper;
using IogServices.Constants;
using IogServices.Models.DTO;
using IogServices.Util;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IogServices.Services.Impl
{
    public class EletraMiddlewareService : IEletraMiddlewareService
    {
        private readonly IMapper _mapper;
        private readonly Middlewares _middlewares;
        public string Manufacturer { get; }
        public EletraMiddlewareService(IMapper mapper, IOptionsMonitor<Middlewares> middlewaresAccessor)
        {
            _mapper = mapper;
            _middlewares = middlewaresAccessor.CurrentValue;
            Manufacturer = "Eletra";
        }
        public ForwarderMessage MakeSmcCreationPackageForForwarder(SmcDto smcDto)
        {
            var forwarderMessage = new ForwarderMessage
            {
                MessageContent = JsonConvert.SerializeObject(
                    new EletraSmcModels.SmcWithMeter {SmcDto = _mapper.Map<SmcDto, EletraSmcModels.SmcDto>(smcDto)}), 
                Uri = _middlewares.EletraSmc.BaseUrl + _middlewares.EletraSmc.Smc.SubRoute
            };

            return forwarderMessage;
        }

        public ForwarderMessage MakeSmcEditionPackageForForwarder(SmcDto smcDto)
        {
            var forwarderMessage = new ForwarderMessage
            {
                MessageContent = "", //todo
                Uri = _middlewares.EletraSmc.BaseUrl + _middlewares.EletraSmc.Smc.SubRoute
            };

            return forwarderMessage;
        }

        public ForwarderMessage MakeSmcDeletionPackageForForwarder(SmcDto smcDto)
        {
            var forwarderMessage = new ForwarderMessage
            {
                MessageContent = "", //todo
                Uri = _middlewares.EletraSmc.BaseUrl + _middlewares.EletraSmc.Smc.SubRoute + "?serial=" + smcDto.Serial
            };
            
            return forwarderMessage;
        }

        public ForwarderMessage MakeMeterWithSmcRelayOnCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto)
        {
            var forwarderMessage = new ForwarderMessage
            {
                MessageContent = "", //todo
                Uri = _middlewares.EletraSmc.BaseUrl
                      + _middlewares.EletraSmc.Meters.SubRoute
                      + _middlewares.EletraSmc.Meters.RelayOn
                      + "?serialSmc=" + meterDto.Smc.Serial
                      + "&serialMeter=" + meterDto.Serial
                      + "&commandId=" + commandTicketDto.CommandId,
                CommandId = commandTicketDto.CommandId
            };

            return forwarderMessage;
        }

        public ForwarderMessage MakeMeterWithSmcRelayOffCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto)
        {
            var forwarderMessage = new ForwarderMessage
            {
                MessageContent = "", //todo
                Uri = _middlewares.EletraSmc.BaseUrl
                      + _middlewares.EletraSmc.Meters.SubRoute
                      + _middlewares.EletraSmc.Meters.RelayOff
                      + "?serialSmc=" + meterDto.Smc.Serial
                      + "&serialMeter=" + meterDto.Serial
                      + "&commandId=" + commandTicketDto.CommandId,
                CommandId = commandTicketDto.CommandId
            };
            
            return forwarderMessage;
        }

        public ForwarderMessage MakeMeterWithSmcRelayStatusCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto)
        {
            var forwarderMessage = new ForwarderMessage
            {
                MessageContent = "", //todo
                Uri = _middlewares.EletraSmc.BaseUrl
                      + _middlewares.EletraSmc.Meters.SubRoute
                      + _middlewares.EletraSmc.Meters.RelayStatus
                      + "?serialSmc=" + meterDto.Smc.Serial
                      + "&serialMeter=" + meterDto.Serial
                      + "&commandId=" + commandTicketDto.CommandId,
                CommandId = commandTicketDto.CommandId
            };
            
            return forwarderMessage;
        }

        public ForwarderMessage MakeMeterCreationPackageForForwarder(MeterDto meterDto)
        {
            ForwarderMessage forwarderMessage;
            
            if (meterDto.Smc != null)
            {
                forwarderMessage = new ForwarderMessage
                {
                    MessageContent = JsonConvert.SerializeObject(new EletraSmcModels.MeterWithSmc
                    {
                        MeterDto = _mapper.Map<MeterDto, EletraSmcModels.MeterDto>(meterDto),
                        Serial = meterDto.Smc.Serial
                    }),
                    Uri = _middlewares.EletraSmc.BaseUrl + _middlewares.EletraSmc.Meters.SubRoute
                };
            }
            else
            {
                forwarderMessage = new ForwarderMessage
                {
                    MessageContent = JsonConvert.SerializeObject(_mapper.Map<EletraSmiModels.MeterDto>(meterDto)),
                    Uri = _middlewares.EletraSmi.BaseUrl + _middlewares.EletraSmi.Meters.SubRoute
                };
            }
            
            
            return forwarderMessage;
        }

        public ForwarderMessage MakeMeterEditionPackageForForwarder(MeterDto meterDto)
        {
            ForwarderMessage forwarderMessage;
            if (meterDto.Smc != null)
            {
                forwarderMessage = new ForwarderMessage
                {
                    MessageContent = JsonConvert.SerializeObject(new EletraSmcModels.MeterWithSmc
                    {
                        MeterDto = _mapper.Map<MeterDto, EletraSmcModels.MeterDto>(meterDto),
                        Serial = meterDto.Smc.Serial
                    }),
                    Uri = _middlewares.EletraSmc.BaseUrl + _middlewares.EletraSmc.Meters.SubRoute
                };
            }
            else
            {
                forwarderMessage = new ForwarderMessage
                {
                    MessageContent = JsonConvert.SerializeObject(_mapper.Map<EletraSmiModels.MeterDto>(meterDto)),
                    Uri = _middlewares.EletraSmi.BaseUrl + _middlewares.EletraSmi.Meters.SubRoute
                };
                
            }
            
            return forwarderMessage;
        }

        public ForwarderMessage MakeMeterDeletionPackageForForwarder(MeterDto meterDto)
        {
            var forwarderMessage = new ForwarderMessage
            {
                MessageContent = ""
            };
            
            if (meterDto.Smc != null)
            {
                forwarderMessage.Uri = _middlewares.EletraSmc.BaseUrl + _middlewares.EletraSmc.Meters.SubRoute
                                                                      + "?serialSmc=" + meterDto.Smc.Serial
                                                                      + "&serialMeter" + meterDto.Serial;
            }
            else
            {
                forwarderMessage.Uri = _middlewares.EletraSmi.BaseUrl + _middlewares.EletraSmi.Meters.SubRoute
                                                                      + "?serial=" + meterDto.Serial;
            }


            return forwarderMessage;
        }

        public ForwarderMessage MakeMeterRelayOnCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto)
        {
            var forwarderMessage = new ForwarderMessage
            {
                MessageContent = "", //todo
                Uri = _middlewares.EletraSmi.BaseUrl
                      + _middlewares.EletraSmi.Meters.SubRoute
                      + _middlewares.EletraSmi.Meters.RelayOn
                      + "?serialMeter=" + meterDto.Serial
                      + "&commandId=" + commandTicketDto.CommandId,
                CommandId = commandTicketDto.CommandId
            };
            
            return forwarderMessage;
        }

        public ForwarderMessage MakeMeterRelayOffCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto)
        {
            var forwarderMessage = new ForwarderMessage
            {
                MessageContent = "", //todo
                Uri = _middlewares.EletraSmi.BaseUrl
                      + _middlewares.EletraSmi.Meters.SubRoute
                      + _middlewares.EletraSmi.Meters.RelayOff
                      + "?serialMeter=" + meterDto.Serial
                      + "&commandId=" + commandTicketDto.CommandId,
                CommandId = commandTicketDto.CommandId
            };
            
            return forwarderMessage;
        }

        public ForwarderMessage MakeMeterRelayStatusCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto)
        {
            var forwarderMessage = new ForwarderMessage
            {
                MessageContent = "", //todo
                Uri = _middlewares.EletraSmi.BaseUrl
                      + _middlewares.EletraSmi.Meters.SubRoute
                      + _middlewares.EletraSmi.Meters.RelayStatus
                      + "?serialMeter=" + meterDto.Serial
                      + "&commandId=" + commandTicketDto.CommandId,
                CommandId = commandTicketDto.CommandId
            };
            
            return forwarderMessage;
        }
    }
}