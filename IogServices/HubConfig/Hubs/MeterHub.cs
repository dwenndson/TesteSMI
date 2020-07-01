using System.Threading.Tasks;
using IogServices.Enums;
using IogServices.Models.DTO;
using IogServices.Services;
using Microsoft.AspNetCore.SignalR;

namespace IogServices.HubConfig.Hubs
{
    public class MeterHub : Hub
    {
        public async Task UpdateForAll(MeterDto meterDto)
        {
            await Clients.All.SendAsync("UpdateForAll", meterDto);
        }
    }
}