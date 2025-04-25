using MediatR;

namespace CalculadoraCedears.Api.Application.CedearsStockHolding.Commands
{
    public class DeleteCedearStockHoldingCommand : IRequest
    {
        public DeleteCedearStockHoldingCommand(Guid cedearStockHoldingId)
        {
            this.CedearStockHoldingId = cedearStockHoldingId;
        }

        public Guid CedearStockHoldingId { get; }
    }
}
