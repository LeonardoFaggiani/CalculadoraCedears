using CalculadoraCedears.Api.Dto.CedearsStockHolding;

using MediatR;

namespace CalculadoraCedears.Api.Application.CedearsStockHolding.Queries
{
    public record CedearsStockHoldingQuery(string UserId) : IRequest<CedearsStockHoldingQueryResponse>
    { }
}
