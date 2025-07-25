﻿using NetDevPack.Domain;

namespace CalculadoraCedears.Api.Domain
{
    public class CedearsStockHolding : Entity
    {
        protected CedearsStockHolding() { }

        public CedearsStockHolding(int quantity,DateTime sinceDate, decimal exchangeRateCcl, decimal purchasePriceArs)
        {
            this.Quantity = quantity;
            this.SinceDate = sinceDate;
            this.ExchangeRateCcl = exchangeRateCcl;
            this.PurchasePriceArs = purchasePriceArs;
        }

        public int Quantity { get; protected set; }
        public decimal EffectiveRatio { get; protected set; }

        public DateTime SinceDate { get; protected set; }

        public DateTime? UntilDate { get; protected set; }

        public decimal ExchangeRateCcl { get; protected set; }

        public decimal PurchasePriceArs { get; protected set; }

        public decimal CurrentValueUsd { get; protected set; }

        public decimal PurchasePriceUsd { get; protected set; }

        public decimal PurchaseValueUsd { get; protected set; }

        public int BrokerId { get; protected set; }

        public Guid CedearId { get; protected set; }
        public int UserId { get; protected set; }

        public virtual Broker Broker { get; protected set; }

        public virtual Cedear Cedear { get; protected set; }
        public virtual User User { get; protected set; }

        public CedearsStockHolding SetCedear(Cedear cedear)
        {
            this.Cedear = cedear;
            return this;
        }

        public CedearsStockHolding SetBroker(Broker broker)
        {
            this.Broker = broker;
            return this;
        }

        public CedearsStockHolding SetUser(User user)
        {
            this.User = user;
            return this;
        }

        public CedearsStockHolding SetEffectiveRatio(int ratio)
        {
            this.EffectiveRatio = (decimal)this.Quantity / ratio;

            return this;
        }

        public CedearsStockHolding SetPurchaseUsd(int ratio, decimal brokerComision)
        {
            decimal comision = (this.PurchasePriceArs * brokerComision) / 100;

            this.PurchasePriceUsd = ((this.PurchasePriceArs + comision)/this.ExchangeRateCcl) * ratio;
            this.PurchaseValueUsd = (this.PurchasePriceUsd * this.EffectiveRatio);

            return this;
        }

        public CedearsStockHolding SetCurrentUsd(decimal price)
        {
            decimal comision = (price * this.Broker.Comision) / 100;
            
            this.Cedear.SetPrice(price);
            this.CurrentValueUsd = ((price - comision) * this.EffectiveRatio);

            return this;
        }        
    }
}
