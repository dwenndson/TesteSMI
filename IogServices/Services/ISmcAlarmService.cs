using System.Collections.Generic;
using IogServices.Models.DTO;

namespace IogServices.Services
{
    public interface ISmcAlarmService : IGenericService<SmcAlarmDto>
    {
        List<SmcAlarmDto> GetBySerial(string serial);
    }
}