using System;

namespace IogServices.Util
{
    public class SendCommandArgs : EventArgs
    {
        public ForwarderMessage Message { get; }

        public SendCommandArgs(ForwarderMessage message)
        {
            Message = message;
        }
    }
}