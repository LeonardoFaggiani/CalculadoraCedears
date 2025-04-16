using NetDevPack.Domain;

namespace CalculadoraCedears.Api.Domian
{
    public class Cedear : Entity
    {
        protected Cedear() { }

        public Cedear(string description, string ticker)
        {
            Description = description;
            Ticker = ticker;
        }

        public string Description { get; }
        public string Ticker { get; }
    }
}
