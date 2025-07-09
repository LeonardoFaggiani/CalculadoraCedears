namespace CalculadoraCedears.Api.Dto.Cedears
{
    public class DollarCCLQueryResponse
    {
        public DollarCCLQueryResponse(decimal dollarCCL, decimal variationCCL)
        {
            this.DollarCCL = dollarCCL;
            this.VariationCCL = variationCCL;
        }

        public decimal DollarCCL { get; }
        public decimal VariationCCL { get; }
    }
}
