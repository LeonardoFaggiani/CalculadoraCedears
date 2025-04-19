namespace CalculadoraCedears.Api.Domain
{
    public class GoogleFinance
    {
        public GoogleFinance(decimal price)
        {
            this.Price = price;
        }

        public decimal Price { get; set; }
    }
}