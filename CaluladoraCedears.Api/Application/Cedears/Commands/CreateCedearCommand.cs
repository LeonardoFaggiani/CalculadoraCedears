using MediatR;

using CaluladoraCedears.Api.Dto.Samples.Request;

namespace CaluladoraCedears.Api.Application.Samples.Commands
{
    public record CreateCedearCommand(CreateCedearRequest request) : IRequest
    { }
}
