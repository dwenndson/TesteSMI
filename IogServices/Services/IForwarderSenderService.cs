using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IogServices.Util;

namespace IogServices.Services
{
    public interface IForwarderSenderService
    {
        Task<HttpResponseMessage> SendPost(ForwarderMessage forwarderMessage);
        HttpResponseMessage SendPut(ForwarderMessage forwarderMessage);
        HttpResponseMessage SendPatch(ForwarderMessage forwarderMessage);
        HttpResponseMessage SendDelete(ForwarderMessage forwarderMessage);
    }
}