using CalculadoraCedears.Api.CrossCutting.Jwt;
using CalculadoraCedears.Api.Dto.Auth.Response;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.Auth.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        
        public CreateUserCommandHandler(IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            Guard.IsNotNull(userRepository, nameof(userRepository));
            Guard.IsNotNull(jwtTokenGenerator, nameof(jwtTokenGenerator));

            this.userRepository = userRepository;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var googleUserInfo = await this.jwtTokenGenerator.ValidateAndGenerateTokenAsync(command.Request.GoogleToken);

            var user = await this.userRepository.All().FirstOrDefaultAsync(x => x.ThirdPartyUserId.ToLower() == googleUserInfo.UserId.ToLower(), cancellationToken);

            if (user is null)
            {
                user = new Domain.User(googleUserInfo.UserId, googleUserInfo.Email);
                this.userRepository.Add(user);
            }
            else
            {
                this.userRepository.Update(user);
            }

            user.SetRefreshToken();
            user.SetLastLogin();

            await this.userRepository.UnitOfWork.Commit();

            return new CreateUserCommandResponse(googleUserInfo.Jwt, user.RefreshToken);
        }
    }
}
