using System;

namespace CalculadoraCedears.Api.Dto
{
    public class CedearStockHoldingDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public DateTime SinceDate { get;  set; }
        public decimal ExchangeRateCcl { get; set; }
        public decimal PurchasePriceArs { get;  set; }
        public decimal PurchasePriceUsd { get; set; }
        public decimal PurchaseValueUsd { get; set; }
        public decimal CurrentPriceUsd { get;  set; }
        public decimal CurrentValueUsd { get;  set; }

        public decimal SinceChange => PurchaseValueUsd == 0 ? 0 : (CurrentValueUsd - PurchaseValueUsd);
        public decimal SinceChangePercent => PurchaseValueUsd == 0 ? 0 : ((CurrentValueUsd / PurchaseValueUsd) - 1) * 100;
    }
}
