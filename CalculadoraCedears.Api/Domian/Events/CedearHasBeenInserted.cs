using NetDevPack.Messaging;

namespace CalculadoraCedears.Api.Domian.Events
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
