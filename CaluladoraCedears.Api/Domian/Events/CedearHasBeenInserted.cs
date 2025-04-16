using NetDevPack.Messaging;

namespace CaluladoraCedears.Api.Domian.Events
{
    public class CedearHasBeenInserted : Event
    {
        public CedearHasBeenInserted(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}
