using System.Runtime.Serialization;

namespace CalculadoraCedears.Api.Infrastructure.Exceptions
{
    [Serializable]
    public class AlreadyExistsCedearException : Exception
    {
        public AlreadyExistsCedearException(string errorMessage) : base(errorMessage)
        { }

        public AlreadyExistsCedearException(string errorMessage, Exception exception) : base(errorMessage, exception)
        { }
        protected AlreadyExistsCedearException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
