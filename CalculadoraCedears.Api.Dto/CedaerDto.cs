using System;

namespace CalculadoraCedears.Api.Dto
{
    public class CedaerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public int Ratio { get; set; }
    }
}
