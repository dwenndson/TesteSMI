using System.Collections.Generic;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using Microsoft.Extensions.Logging;

namespace IogServices.Services
{
    public interface IDeviceLogService
    {
        List<DeviceLogDto> GetBySerial(string serial);
        List<DeviceLogDto> GetBySerialFilterByLogLevel(string serial, LogLevel level);

        DeviceLog Save(DeviceLog deviceLog);

    }
}