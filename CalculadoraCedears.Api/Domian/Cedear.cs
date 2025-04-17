using NetDevPack.Domain;

namespace CalculadoraCedears.Api.Domian
{
    public class Cedear : Entity
    {
        protected Cedear() { }

        public Cedear(string name,
            string ticker,
            int ratio)
        {
            this.Name = name;
            this.Ticker = ticker;
            this.Ratio = ratio;
            this.CedearsStockHoldings = new List<CedearsStockHolding>();
        }


        public string Name { get; protected set; }

        public string Ticker { get; protected set; }

        public int Ratio { get; protected set; }

        public virtual ICollection<CedearsStockHolding> CedearsStockHoldings { get; protected set; }   
    }
}
