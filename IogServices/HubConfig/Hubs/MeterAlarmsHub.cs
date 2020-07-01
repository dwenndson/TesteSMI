using System.Threading.Tasks;
using IogServices.Enums;
using IogServices.Models.DTO;
using Microsoft.AspNetCore.SignalR;

namespace IogServices.HubConfig.Hubs
{
    public class MeterAlarmHub : Hub
    {
        public async Task UpdateForAll(MeterAlarmDto meterAlarmdto)
        {
            await Clients.All.SendAsync("UpdateForAll", meterAlarmdto);
        }
    }
}
