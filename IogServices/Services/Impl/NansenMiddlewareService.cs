using System;
using AutoMapper;
using IogServices.Constants;
using IogServices.Models.DTO;
using IogServices.Util;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IogServices.Services.Impl
{
    public class NansenMiddlewareService : INansenMiddlewareService
    {
        private readonly IMapper _mapper;
        private readonly Middlewares _middlewares;
        public string Manufacturer { get; }
        public NansenMiddlewareService(IMapper mapper, IOptionsMonitor<Middlewares> middlewaresAccessor)
        {
            _mapper = mapper;
            _middlewares = middlewaresAccessor.CurrentValue;
            Manufacturer = "Nansen";
        }
        public ForwarderMessage MakeSmcCreationPackageForForwarder(SmcDto smcDto)
        {
            throw new NotImplementedException();
        }

        public ForwarderMessage MakeSmcEditionPackageForForwarder(SmcDto smcDto)
        {
            throw new NotImplementedException();

        }

        public ForwarderMessage MakeSmcDeletionPackageForForwarder(SmcDto smcDto)
        {
            throw new NotImplementedException();

        }

        public ForwarderMessage MakeMeterWithSmcRelayOnCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto)
        {
            throw new NotImplementedException();

        }

        public ForwarderMessage MakeMeterWithSmcRelayOffCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto)
        {
            throw new NotImplementedException();

        }

        public ForwarderMessage MakeMeterWithSmcRelayStatusCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto)
        {
            throw new NotImplementedException();

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
                    Uri = _middlewares.NansenSmi.BaseUrl + _middlewares.NansenSmi.Meters.SubRoute
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
                    Uri = _middlewares.NansenSmi.BaseUrl + _middlewares.NansenSmi.Meters.SubRoute
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
                forwarderMessage.Uri = _middlewares.NansenSmi.BaseUrl + _middlewares.NansenSmi.Meters.SubRoute
                                                                      + "?serial=" + meterDto.Serial;
            }


            return forwarderMessage;
        }

        public ForwarderMessage MakeMeterRelayOnCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto)
        {
            throw new System.NotImplementedException();
        }

        public ForwarderMessage MakeMeterRelayOffCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto)
        {
            throw new System.NotImplementedException();
        }

        public ForwarderMessage MakeMeterRelayStatusCommandForForwarder(MeterDto meterDto,
            CommandTicketDto commandTicketDto)
        {
            throw new System.NotImplementedException();
        }
    }
}