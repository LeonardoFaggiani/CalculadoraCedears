using CalculadoraCedears.Api.Dto.CedearsStockHolding.Request;

using MediatR;

namespace CalculadoraCedears.Api.Application.CedearsStockHolding.Commands
{
    public record UpdateCedearStockHoldingCommand(UpdateCedearStockHoldingRequest request) : IRequest
    { }
}
