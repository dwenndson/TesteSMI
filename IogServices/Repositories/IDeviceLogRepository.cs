using System.Collections.Generic;
using IogServices.Models.DAO;
using Microsoft.Extensions.Logging;

namespace IogServices.Repositories
{
    public interface IDeviceLogRepository : IGenericRepository<DeviceLog>
    {
        List<DeviceLog> GetBySerial(string serial);
        List<DeviceLog> GetBySerialFilterByLogLevel(string serial, LogLevel level);

    }
}