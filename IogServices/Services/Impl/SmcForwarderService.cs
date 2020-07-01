using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using IogServices.Models.DTO;
using Smc = IogServices.Models.DAO.Smc;

namespace IogServices.Services.Impl
{
    public class SmcForwarderService : ISmcForwarderService
    {
        private readonly IMiddlewareProviderService _middlewareProviderService;
        private readonly IForwarderSenderService _forwarderSenderService;
        private readonly ISmcService _smcService;
        private readonly IMapper _mapper;

        public SmcForwarderService(
            IMapper mapper,
            ISmcService smcService,
            IForwarderSenderService forwarderSenderService,
            IMiddlewareProviderService middlewareProviderService)
        {
            _forwarderSenderService = forwarderSenderService;
            _middlewareProviderService = middlewareProviderService;
            _smcService = smcService;
            _mapper = mapper;
        }

        public HttpResponseMessage ForwardCreation(SmcDto t)
        {
            try
            {
                var middlewareService =
                    _middlewareProviderService.GetSmcMiddlewareServiceByManufacturerName(t.SmcModel.Manufacturer.Name);
                var convertedMessage = middlewareService.MakeSmcCreationPackageForForwarder(t);
                return _forwarderSenderService.SendPost(convertedMessage).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public HttpResponseMessage ForwardEdition(SmcDto t)
        {
            try
            {
                var middlewareService =
                    _middlewareProviderService.GetSmcMiddlewareServiceByManufacturerName(t.SmcModel.Manufacturer.Name);
                var convertedMessage = middlewareService.MakeSmcEditionPackageForForwarder(t);
                return _forwarderSenderService.SendPatch(convertedMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public HttpResponseMessage ForwardDeletion(SmcDto smcDto)
        {
            try
            {
                var middlewareService =
                    _middlewareProviderService.GetSmcMiddlewareServiceByManufacturerName(smcDto.SmcModel.Manufacturer.Name);
                var convertedMessage = middlewareService.MakeSmcDeletionPackageForForwarder(smcDto);
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