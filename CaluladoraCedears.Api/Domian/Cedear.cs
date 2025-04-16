using NetDevPack.Domain;

namespace CaluladoraCedears.Api.Domian
{
    public class Cedear : Entity
    {
        protected Cedear() { }

        public Cedear(string description, string ticker)
        {
            this.Description = description;
            this.Ticker = ticker;
        }

        public string Description { get; }
        public string Ticker { get; }
    }
}
