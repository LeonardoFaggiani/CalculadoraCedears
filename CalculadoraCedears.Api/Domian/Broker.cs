namespace CalculadoraCedears.Api.Domian;

public partial class Broker
{
    protected Broker() { }

    public Broker(string name, decimal comision)
    {
        this.Name = name;
        this.Comision = comision;
        this.CedearsStockHoldings = new List<CedearsStockHolding>();
    }

    public int Id { get; set; }

    public string Name { get; protected set; }

    public decimal Comision { get; protected set; }

    public virtual ICollection<CedearsStockHolding> CedearsStockHoldings { get; protected set; }
}