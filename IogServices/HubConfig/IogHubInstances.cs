using IogServices.HubConfig.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace IogServices.HubConfig
{
    public class IogHubInstances
    {
        public IHubContext<GeneralHubObject> GeneralHubContext { get; set; }
    }
}