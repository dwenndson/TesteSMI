using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IogServices.Enums;
using IogServices.Models.DTO;
using Microsoft.AspNetCore.SignalR;

namespace IogServices.HubConfig.Hubs
{
    public class DeviceLogHub : Hub
    {
        public async Task UpdateForAll(DeviceLogDto deviceLogDto)
        {
            await Clients.All.SendAsync("UpdateForAll", deviceLogDto);
        }
    }
}
