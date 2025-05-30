using System;

namespace CalculadoraCedears.Api.Dto.CedearsStockHolding.Request
{
    public class CedearStockHoldingRequest
    {
        public string UserId {  get; set; }
        public Guid CedearId { get; set; }
        public int BrokerId { get; set; }
        public int Quantity { get; set; }
        public DateTime SinceDate { get; set; }
        public decimal ExchangeRateCcl { get; set; }
        public decimal PurchasePriceArs { get; set; }
    }
}