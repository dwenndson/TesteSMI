using System.Threading.Tasks;
using IogServices.Enums;
using IogServices.Models.DTO;
using Microsoft.AspNetCore.SignalR;

namespace IogServices.HubConfig.Hubs
{
    public class EnergyHub : Hub
    {
        public async Task UpdateForAll(MeterEnergyDto meterEnergyDto)
        {
            await Clients.All.SendAsync("UpdateForAll", meterEnergyDto);
        }
    }
}