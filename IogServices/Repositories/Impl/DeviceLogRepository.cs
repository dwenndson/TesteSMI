using System;
using System.Collections.Generic;
using System.Linq;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using Microsoft.Extensions.Logging;

namespace IogServices.Repositories.Impl
{
    public class DeviceLogRepository : IDeviceLogRepository
    {
        private readonly IogContext _iogContext;
        
        public DeviceLogRepository(IogContext iogContext)
        {
            _iogContext = iogContext;
        }
        public List<DeviceLog> GetAll()
        {
            return _iogContext.DeviceLogs.ToList();
        }

        public DeviceLog GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public DeviceLog Save(DeviceLog t)
        {
            _iogContext.DeviceLogs.Add(t);
            _iogContext.SaveChanges();
            return t;
        }

        public DeviceLog Update(DeviceLog t)
        {
            _iogContext.Update(t);
            _iogContext.SaveChanges();
            return t;
        }

        public List<DeviceLog> GetBySerial(string serial)
        {
            return _iogContext.DeviceLogs.Where(d => d.DeviceSerial == serial).OrderByDescending(d=>d.CreatedAt).ToList();
        }

        public List<DeviceLog> GetBySerialFilterByLogLevel(string serial, LogLevel level)
        {
            return _iogContext.DeviceLogs.Where(d => d.DeviceSerial == serial && d.LogLevel == level).OrderByDescending(d=>d.CreatedAt).ToList();
        }
    }
}