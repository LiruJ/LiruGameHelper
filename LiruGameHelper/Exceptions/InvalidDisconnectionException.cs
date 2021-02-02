using System;

namespace LiruGameHelper.Exceptions
{

    [Serializable]
    public class InvalidDisconnectionException : Exception
    {
        public InvalidDisconnectionException() { }
        public InvalidDisconnectionException(string message) : base(message) { }
        public InvalidDisconnectionException(string message, Exception inner) : base(message, inner) { }
        protected InvalidDisconnectionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
