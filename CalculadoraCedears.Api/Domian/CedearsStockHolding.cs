using NetDevPack.Domain;

namespace CalculadoraCedears.Api.Domian
{
    public class CedearsStockHolding : Entity
    {
        protected CedearsStockHolding() { }

        public CedearsStockHolding(int quantity,DateTime sinceDate, decimal exchangeRateCcl, decimal purchasePriceArs)
        {
            this.Quantity = quantity;
            this.EffectiveRatio = quantity;
            this.SinceDate = sinceDate;
            this.ExchangeRateCcl = exchangeRateCcl;
            this.PurchasePriceArs = purchasePriceArs;
        }

        public int Quantity { get; protected set; }
        public decimal EffectiveRatio { get; protected set; }

        public decimal TodayChange { get; protected set; }

        public decimal TodayChangePercent { get; protected set; }

        public decimal SinceChange { get; protected set; }

        public decimal SinceChangePercent { get; protected set; }

        public DateTime SinceDate { get; protected set; }

        public DateTime? UntilDate { get; protected set; }

        public decimal ExchangeRateCcl { get; protected set; }

        public decimal PurchasePriceArs { get; protected set; }

        public decimal CurrentPriceUsd { get; protected set; }

        public decimal CurrentValueUsd { get; protected set; }

        public decimal PurchasePriceUsd { get; protected set; }

        public decimal PurchaseValueUsd { get; protected set; }

        public int BrokerId { get; protected set; }

        public Guid CedearId { get; protected set; }

        public virtual Broker Broker { get; protected set; }

        public virtual Cedear Cedear { get; protected set; }

        public void SetCedear(Cedear cedear)
        {
            this.Cedear = Cedear;
        }

        public void SetBroker(Broker broker)
        {
            this.Broker = broker;
        }
    }
}
