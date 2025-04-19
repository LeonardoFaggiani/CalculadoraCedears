using System.Runtime.Serialization;

namespace CalculadoraCedears.Api.Infrastructure.Exceptions
{
    [Serializable]
    public class GoogleFinancePriceNotFoundException : Exception
    {
        public GoogleFinancePriceNotFoundException(string errorMessage) : base(errorMessage)
        { }

        public GoogleFinancePriceNotFoundException(string errorMessage, Exception exception) : base(errorMessage, exception)
        { }
        protected GoogleFinancePriceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
