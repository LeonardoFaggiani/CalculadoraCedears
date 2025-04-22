using CalculadoraCedears.Api.Dto.Cedears;

using MediatR;

namespace CalculadoraCedears.Api.Application.Cedears.Queries
{
    public class CedearsQuery : IRequest<CedearsQueryResponse>
    {
        public string Ticker { get; set; }
    }
}