using System;
using System.Collections.Generic;
using System.Linq;

namespace IogServices.Services.Impl
{
    public class MiddlewareProviderService : IMiddlewareProviderService
    {
        private readonly IList<IMeterMiddlewareService> _meterMiddlewareServices = new List<IMeterMiddlewareService>();
        private readonly IList<ISmcMiddlewareService> _smcMiddlewareServices = new List<ISmcMiddlewareService>();

        public MiddlewareProviderService(IEletraMiddlewareService eletraMiddlewareService, INansenMiddlewareService nansenMiddlewareService)
        {
            _meterMiddlewareServices.Add(eletraMiddlewareService);
            _meterMiddlewareServices.Add(nansenMiddlewareService);

            _smcMiddlewareServices.Add(eletraMiddlewareService);
            _smcMiddlewareServices.Add(nansenMiddlewareService);

        }
        
        public ISmcMiddlewareService GetSmcMiddlewareServiceByManufacturerName(string manufacturerName)
        {
            var service = _smcMiddlewareServices.First(x =>
                string.Equals(x.Manufacturer, manufacturerName, StringComparison.CurrentCultureIgnoreCase));
            if (service != null) return service;
            throw new NotImplementedException($"Fabricante {manufacturerName} não existe ou não possui smc");
        }

        public IMeterMiddlewareService GetMeterMiddlewareServiceByManufacturerName(string manufacturerName)
        {
            var service = _meterMiddlewareServices.First(x =>
                string.Equals(x.Manufacturer, manufacturerName, StringComparison.CurrentCultureIgnoreCase));
            if (service != null) return service;
            throw new NotImplementedException($"Fabricante {manufacturerName} não existe ou não possui medidores");
        }
    }
}