using System;
using System.Net;
using System.Net.Http;
using AutoMapper;
using IogServices.Models.DAO;
using IogServices.Models.DTO;

namespace IogServices.Services.Impl
{
    public class MeterForwarderService : IMeterForwarderService
    {
        private readonly IMiddlewareProviderService _middlewareProviderService;
        private readonly IForwarderSenderService _forwarderSenderService;
        private readonly IMapper _mapper;

        public MeterForwarderService(
            IMapper mapper,
            IForwarderSenderService forwarderSenderService,
            IMiddlewareProviderService middlewareProviderService)
        {
            _forwarderSenderService = forwarderSenderService;
            _middlewareProviderService = middlewareProviderService;
            _mapper = mapper;
        }
        public HttpResponseMessage ForwardCreation(MeterDto t)
        {
            try
            {
                var middlewareService =
                    _middlewareProviderService.GetMeterMiddlewareServiceByManufacturerName(t.MeterModel.Manufacturer
                        .Name);
                var convertedMessage = middlewareService.MakeMeterCreationPackageForForwarder(t);
                return _forwarderSenderService.SendPost(convertedMessage).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public HttpResponseMessage ForwardEdition(MeterDto t)
        {
            try
            {
                var middlewareService =
                    _middlewareProviderService.GetMeterMiddlewareServiceByManufacturerName(t.MeterModel.Manufacturer
                        .Name);
                var convertedMessage = middlewareService.MakeMeterEditionPackageForForwarder(t);
                return _forwarderSenderService.SendPatch(convertedMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public HttpResponseMessage ForwardDeletion(MeterDto meterDto)
        {
            try
            {
                var middlewareService =
                    _middlewareProviderService.GetMeterMiddlewareServiceByManufacturerName(meterDto.MeterModel
                        .Manufacturer.Name);
                var convertedMessage = middlewareService.MakeMeterDeletionPackageForForwarder(meterDto);
                return _forwarderSenderService.SendDelete(convertedMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}