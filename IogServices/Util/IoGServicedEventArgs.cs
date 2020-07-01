using System;

namespace IogServices.Util
{
    public class IoGServicedEventArgs<T> : EventArgs
    {
        public T ObjectFromEvent { get; }
        
        public IoGServicedEventArgs(T objectFromEvent)
        {
            ObjectFromEvent = objectFromEvent;
        }
    }
}