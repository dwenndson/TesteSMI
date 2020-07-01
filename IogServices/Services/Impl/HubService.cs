using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IogServices.Enums;
using IogServices.Models.DTO;
using IogServices.HubConfig;
using Microsoft.AspNetCore.SignalR;

namespace IogServices.Services.Impl
{
    public class HubService : IHubService
    {
        private readonly IogHubInstances _iogHubInstances = Startup.IogHubInstances;
        
        
        public async Task GeneralSendUpdate(string method, object obj)
        {
            await _iogHubInstances.GeneralHubContext.Clients.All.SendAsync(method, obj);
        }
    }
}
