using CalculadoraCedears.Api.Dto.Users.Request;
using CalculadoraCedears.Api.Dto.Users.Response;

using MediatR;

namespace CalculadoraCedears.Api.Application.Users.Commands
{
    public record CreateUserCommand(UserRequest Request) : IRequest<CreateUserCommandResponse>
    { }
}
