using CalculadoraCedears.Api.CrossCutting.Jwt;
using CalculadoraCedears.Api.Dto.Auth.Response;

using CommunityToolkit.Diagnostics;

using MediatR;

namespace CalculadoraCedears.Api.Application.Auth.Commands
{
    public class CreateJwtTokenCommandHandler : IRequestHandler<CreateJwtTokenCommand, CreateJwtTokenCommandResponse>
    {
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public CreateJwtTokenCommandHandler(IJwtTokenGenerator jwtTokenGenerator)
        {       
            Guard.IsNotNull(jwtTokenGenerator, nameof(jwtTokenGenerator));

            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<CreateJwtTokenCommandResponse> Handle(CreateJwtTokenCommand command, CancellationToken cancellationToken)
        {
            var googleUserInfo = await this.jwtTokenGenerator.ValidateRefreshTokenAsync(command.Request.AccessToken, command.Request.RefreshToken);

            return new CreateJwtTokenCommandResponse(googleUserInfo.Jwt, googleUserInfo.RefreshToken);
        }
    }
}
