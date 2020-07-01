using System.Threading;

namespace IogServices.Util
{
    public class CommandDeviceTopic
    {
        public string DeviceSerial { get; set; }
        public string DeviceTopic { get; set; }
        public readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
    }
}