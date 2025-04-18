using CalculadoraCedears.Api.Dto.Cedears;

using MediatR;

namespace CalculadoraCedears.Api.Application.Cedears.Queries
{
    public class SearchCedearsByTickerQuery : IRequest<SearchCedearsByTickerQueryResponse>
    {
        public string Ticker { get; set; }
    }
}