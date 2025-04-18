using CalculadoraCedears.Api.Dto.Cedears.Request;

using MediatR;

namespace CalculadoraCedears.Api.Application.Cedears.Commands
{
    public record CreateCedearCommand(CreateCedearRequest request) : IRequest
    { }
}
