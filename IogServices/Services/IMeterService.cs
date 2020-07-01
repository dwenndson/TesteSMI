using System;
using System.Collections.Generic;
using IogServices.Models.DAO;
using IogServices.Models.DTO;
using NetworkObjects.Enum;

namespace IogServices.Services
{
    public interface IMeterService : IGenericService<MeterDto>
    {
        MeterDto GetBySerial(string serial);
        void Deactivate(string serial);
        void SetModem(string serial, string eui);
        Meter GetExistingMeter(string serial);
        List<MeterDto> GetMetersBySmc(string serial);
        List<MeterDto> GetMetersComissioned();
        List<MeterDto> GetMetersNotComissioned();
        void ChangeComissionedStatus(string serial, bool status);
        List<MeterDto> GetAllNotBelongingToASmc();
        void SetRelayStatus(string serial, AccountantStatus accountantStatus);



    }
}