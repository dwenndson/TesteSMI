namespace IogServices.Services
{
    public interface IMiddlewareProviderService
    {
        ISmcMiddlewareService GetSmcMiddlewareServiceByManufacturerName(string manufacturerName);
        IMeterMiddlewareService GetMeterMiddlewareServiceByManufacturerName(string manufacturerName);
    }
}