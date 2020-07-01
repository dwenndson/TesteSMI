using System;

namespace IogServices.Util
{
    public class ForwarderMessage
    {
        public string MessageContent { get; set; }
        public string Uri { get; set; }
        public Guid CommandId { get; set; }
    }
}