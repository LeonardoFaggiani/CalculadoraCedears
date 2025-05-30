using CalculadoraCedears.Api.Dto.Users.Request;

using MediatR;

namespace CalculadoraCedears.Api.Application.Users.Commands
{
    public record CreateUserCommand(UserRequest Request) : IRequest
    { }
}
