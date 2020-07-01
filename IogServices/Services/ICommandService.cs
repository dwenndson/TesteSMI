using IogServices.Models.DTO;
using IogServices.Util;

namespace IogServices.Services
{
    public interface ICommandService
    {
        void RelayOn(string serial, string token);
        void RelayOff(string serial, string token);
        void GetRelay(string serial, string token);
    }
}