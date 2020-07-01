namespace IogServices.Util.Impl
{
    public class CommandStatus
    {
        public int Type = 2;
        public string Serial;
        public string Status;
        public string UpdatedAt;
        
        public CommandStatus(string serial, string commandStatus, string updatedAt)
        {
            this.Serial = serial;
            this.Status = commandStatus;
            this.UpdatedAt = updatedAt;
        }
    }
}