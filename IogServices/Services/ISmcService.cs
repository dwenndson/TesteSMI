using System.Collections.Generic;
using IogServices.Models.DAO;
using IogServices.Models.DTO;

namespace IogServices.Services
{
    public interface ISmcService : IGenericService<SmcDto>
    {
        SmcDto GetBySerial(string serial);
        void Deactivate(string serial);
        void SetModem(string serial, string eui);
        Smc GetExistingSmc(string serial);
        Smc GetSmc(string serial);
        List<MeterDto> GetMeterBySmc(string serial);
        List<SmcDto> GetAllComissioned();
        List<SmcDto> GetAllNotComissioned();
        void ChangeComissionedStatus(string serial, bool status);


    }
}