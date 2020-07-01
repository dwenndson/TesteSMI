
using IogServices.Enums;
using IogServices.HubConfig.PayloadHub;


namespace IogServices.HubConfig
{
    public class CommandHub
    {
        public CommandTypeHub Type;
        public PayloadCommandHub payload;

        public CommandHub(CommandTypeHub type, PayloadCommandHub payloadHub)
        {
            this.Type = type;
            this.payload = payloadHub;
        }

    }
}