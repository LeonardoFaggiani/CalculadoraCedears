using CalculadoraCedears.Api.Application.Base;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.Users.Commands
{
    public class CreateUserCommandHandler : CommandHandler<CreateUserCommand>
    {
        private readonly IUserRepository userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            Guard.IsNotNull(userRepository, nameof(userRepository));

            this.userRepository = userRepository;
        }
        protected override async Task Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await this.userRepository.All().FirstOrDefaultAsync(x => x.ThirdPartyUserId.ToLower() == command.Request.UserId.ToLower(), cancellationToken);

            if (user is null)
                user = new Domain.User(command.Request.UserId, command.Request.Email);

            user.SetLastLogin();

            this.userRepository.Add(user);

            await Commit(this.userRepository.UnitOfWork);
        }
    }
}
