using CalculadoraCedears.Api.Dto.Auth.Request;
using CalculadoraCedears.Api.Dto.Auth.Response;

using MediatR;

namespace CalculadoraCedears.Api.Application.Auth.Commands
{
    public record CreateUserCommand(UserRequest Request) : IRequest<CreateUserCommandResponse>
    { }
}
