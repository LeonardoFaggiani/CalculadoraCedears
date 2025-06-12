using NetDevPack.Domain;

namespace CalculadoraCedears.Api.Domain
{
    public class Cedear : Entity
    {
        protected Cedear() { }

        public Cedear(string name,
            string ticker,
            string market,
            int ratio)
        {
            this.Name = name;
            this.Ticker = ticker;
            this.Market = market;
            this.Ratio = ratio;
            this.CedearsStockHoldings = new List<CedearsStockHolding>();
        }

        public string Name { get; protected set; }
        public string Ticker { get; protected set; }
        public string Market { get; protected set; }
        public int Ratio { get; protected set; }
        public decimal? Price { get; protected set; }
        public bool PriceHasBeenChanged { get; protected set; }
        public virtual ICollection<CedearsStockHolding> CedearsStockHoldings { get; protected set; }

        public void SetPrice(decimal price)
        {
            this.Price = price;
        }

        public bool SetPriceHasBeenChanged(bool isChanged) =>
            this.PriceHasBeenChanged = isChanged;
    }
}
