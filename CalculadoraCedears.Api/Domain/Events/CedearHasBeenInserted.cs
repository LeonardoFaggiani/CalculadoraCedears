using NetDevPack.Messaging;

namespace CalculadoraCedears.Api.Domain.Events
{
    public class CedearHasBeenInserted : Event
    {
        public CedearHasBeenInserted(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
