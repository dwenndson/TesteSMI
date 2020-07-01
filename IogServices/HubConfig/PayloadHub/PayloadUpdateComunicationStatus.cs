
using NetworkObjects.Enum;

namespace IogServices.HubConfig.PayloadHub
{
    public class PayloadUpdateComunicationStatus : PayloadCommandHub
    {
        public CommunicationStatus CommunicationStatus { get; set; }
       

        public PayloadUpdateComunicationStatus(string serial, CommunicationStatus communicationStatus, string updateAt)
        {
            this.Serial = serial;
            this.CommunicationStatus = communicationStatus;
            this.UpdateAt = updateAt;
        }
    }
}
