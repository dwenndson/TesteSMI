using System.IdentityModel.Tokens.Jwt;

namespace IogServices.Util
{
    public class CommandQueue
    {
        public int Type;
        public string Serial;
        public bool IncrementQueue;
        public bool DecrementQueue;
        public string UpdatedAt;
        
        public CommandQueue(int type, string serial, bool incrementQueue, bool decrementQueue, string updatedAt)
        {
            this.Type = type;
            this.Serial = serial;
            this.IncrementQueue = incrementQueue;
            this.DecrementQueue = decrementQueue;
            this.UpdatedAt = updatedAt;
        }
    }
}