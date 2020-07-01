using System;
using MqttClientLibrary.Models;

namespace IogServices.Util
{
    public class MiddlewareMessageArgs : EventArgs
    {
        public Message MessageReceived { get; set; }

        public MiddlewareMessageArgs(Message messageReceived)
        {
            MessageReceived = messageReceived;
        }
    }
}