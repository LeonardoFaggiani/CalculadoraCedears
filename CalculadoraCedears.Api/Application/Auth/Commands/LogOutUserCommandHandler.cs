using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Application.Auth.Commands
{
    public class LogOutUserCommandHandler : AsyncRequestHandler<LogOutUserCommand>
    {
        private readonly IUserRepository userRepository;

        public LogOutUserCommandHandler(IUserRepository userRepository)
        {
            Guard.IsNotNull(userRepository, nameof(userRepository));

            this.userRepository = userRepository;
        }

        protected override async Task Handle(LogOutUserCommand request, CancellationToken cancellationToken)
        {
            var user = await this.userRepository.All().FirstOrDefaultAsync(x => x.ThirdPartyUserId == request.LogOutUserRequest.UserId, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("Refresh token expirado");

            user.SetLogOut();

            this.userRepository.Update(user);

            await this.userRepository.UnitOfWork.Commit();
        }
    }
}
