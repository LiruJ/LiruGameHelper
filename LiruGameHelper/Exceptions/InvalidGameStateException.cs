using System;
using System.Runtime.Serialization;

namespace LiruGameHelper.Exceptions
{
    /// <summary> Should be used for when the game tries to execute a command in the wrong state. For example, the game trying to open the inventory when on the main menu. </summary>
    [Serializable]
    public class InvalidGameStateException : Exception
    {
        public InvalidGameStateException() { }
        public InvalidGameStateException(string message) : base(message) { }
        public InvalidGameStateException(string message, Exception inner) : base(message, inner) { }
        protected InvalidGameStateException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }
}
