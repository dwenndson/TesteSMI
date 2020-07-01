using System.Threading.Tasks;
using IogServices.Enums;
using IogServices.Models.DTO;
using Microsoft.AspNetCore.SignalR;

namespace IogServices.HubConfig.Hubs
{
    public class SmcHub : Hub
    {
        public async Task UpdateForAll(SmcDto smcDto)
        {
            await Clients.All.SendAsync("UpdateForAll", smcDto);
        }
    }
}