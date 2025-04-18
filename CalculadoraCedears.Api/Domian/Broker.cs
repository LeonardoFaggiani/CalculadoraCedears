using NetDevPack.Domain;

namespace CalculadoraCedears.Api.Domian;

public partial class Broker : Entity
{
    protected Broker() { }

    public Broker(string name, decimal comision)
    {
        this.Name = name;
        this.Comision = comision;
        this.CedearsStockHoldings = new List<CedearsStockHolding>();
    }

    public new int Id { get; set; }

    public string Name { get; protected set; }

    public decimal Comision { get; protected set; }

    public virtual ICollection<CedearsStockHolding> CedearsStockHoldings { get; protected set; }
}