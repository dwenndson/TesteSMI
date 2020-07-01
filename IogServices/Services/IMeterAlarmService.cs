using System.Collections.Generic;
using IogServices.Models.DTO;
using NetworkObjects;

namespace IogServices.Services
{
    public interface IMeterAlarmService : IGenericService<MeterAlarmDto>
    {
        
        List<MeterAlarmDto> GetBySerial(string serial);
        void ProcessMessage(AlarmMeterDto meter);

    }
}