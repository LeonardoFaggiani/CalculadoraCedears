using System;

namespace CalculadoraCedears.Api.Dto.CedearsStockHolding.Request
{
    public class CedearStockHoldingRequest
    {
        public Guid CedearId { get; set; }
        public int BrokerId { get; set; }
        public int Quantity { get; set; }
        public DateTime SinceDate { get; set; }
        public decimal ExchangeRateCCL { get; set; }
        public decimal PurchasePriceArs { get; set; }
    }
}