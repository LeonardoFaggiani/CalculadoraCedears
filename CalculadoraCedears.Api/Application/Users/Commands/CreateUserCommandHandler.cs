using CalculadoraCedears.Api.CrossCutting.Jwt;
using CalculadoraCedears.Api.Dto.Users.Response;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.Users.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly IConfiguration configuration;
        
        public CreateUserCommandHandler(IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IConfiguration configuration)
        {
            Guard.IsNotNull(userRepository, nameof(userRepository));
            Guard.IsNotNull(jwtTokenGenerator, nameof(jwtTokenGenerator));
            Guard.IsNotNull(configuration, nameof(configuration));

            this.userRepository = userRepository;
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.configuration = configuration;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var encodeJwt = await this.jwtTokenGenerator.ValidateAndGenerateTokenAsync(command.Request.Email,
                          command.Request.UserId,
                          command.Request.GoogleToken,
                          this.configuration["GoogleClient:Id"]);

            var user = await this.userRepository.All().FirstOrDefaultAsync(x => x.ThirdPartyUserId.ToLower() == command.Request.UserId.ToLower(), cancellationToken);

            if (user is null)
            {
                user = new Domain.User(command.Request.UserId, command.Request.Email);
                this.userRepository.Add(user);
            }
            else
            {
                this.userRepository.Update(user);
            }

            user.SetLastLogin();

            await this.userRepository.UnitOfWork.Commit();

            return new CreateUserCommandResponse(encodeJwt);
        }
    }
}
