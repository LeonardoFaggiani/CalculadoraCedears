using System;

namespace CalculadoraCedears.Api.Dto
{
    public class CedearDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public int Ratio { get; set; }
        public decimal Price { get; set; }
    }
}
