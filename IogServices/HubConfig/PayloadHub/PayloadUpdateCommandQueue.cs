namespace IogServices.HubConfig.PayloadHub
{
    public class PayloadUpdateCommandQueue : PayloadCommandHub
    {
        public int Value { get; set; }


        public PayloadUpdateCommandQueue(string serial, int value, string updatedAt)
        {
            this.Serial = serial;
            this.UpdateAt = updatedAt;
            this.Value = value;
        }
    }
}