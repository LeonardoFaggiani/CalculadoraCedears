using MediatR;
using CalculadoraCedears.Api.Dto.Samples.Request;

namespace CalculadoraCedears.Api.Application.Cedears.Commands
{
    public record CreateCedearCommand(CreateCedearRequest request) : IRequest
    { }
}
