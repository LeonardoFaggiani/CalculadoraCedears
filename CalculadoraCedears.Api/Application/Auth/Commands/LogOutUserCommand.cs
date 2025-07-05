using CalculadoraCedears.Api.Dto.Auth.Request;

using MediatR;

namespace CalculadoraCedears.Api.Application.Auth.Commands
{
    public record LogOutUserCommand(LogOutUserRequest LogOutUserRequest) : IRequest
    { }
}
