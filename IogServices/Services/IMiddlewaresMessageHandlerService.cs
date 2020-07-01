using MqttClientLibrary.Models;

namespace IogServices.Services
{
    public interface IMiddlewaresMessageHandlerService
    {
        void MessageReceivedFromMiddlewares(object sender, MessageReceivedArgs messageReceived);
    }
}