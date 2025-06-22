using CalculadoraCedears.Api.Application.Users.Commands;
using CalculadoraCedears.Api.CrossCutting.Jwt;
using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Dto.Users.Request;
using CalculadoraCedears.Api.Infrastructure.Repositories;
using CalculadoraCedears.Api.Unit.Tests.Base;

using Microsoft.Extensions.Configuration;

using MockQueryable;

using NetDevPack.Data;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Users.Commands
{
    public class CreateUserCommandHandlerTests : BaseTestClass<CreateUserCommandHandler>
    {
        private readonly IUserRepository UserRepository;
        private readonly IJwtTokenGenerator JwtTokenGenerator;
        private readonly IConfiguration Configuration;
        private CreateUserCommand Command;

        public CreateUserCommandHandlerTests()
        {
            this.Configuration = Mock.Of<IConfiguration>();
            this.JwtTokenGenerator = Mock.Of<IJwtTokenGenerator>();
            this.UserRepository = Mock.Of<IUserRepository>(x=> x.UnitOfWork == Mock.Of<IUnitOfWork>());

            var users = new List<User> { new User("123", "test@test.com") };
            var googleFinance = new GoogleFinance(299M);
            var request = new UserRequest()
            {
                UserId = "123"
            };

            this.Command = new CreateUserCommand(request);

            Mock.Get(this.UserRepository).Setup(x => x.All()).Returns(users.AsQueryable().BuildMock());
            Mock.Get(this.JwtTokenGenerator).Setup(x => x.ValidateAndGenerateTokenAsync(It.IsAny<string>())).ReturnsAsync(new GoogleUserInfo("1","test@test","jwt"));

            Sut = new CreateUserCommandHandler(this.UserRepository, this.JwtTokenGenerator);
        }

        public class The_Constructor : CreateUserCommandHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_userRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CreateUserCommandHandler(null, this.JwtTokenGenerator));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_jwtTokenGenerator_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CreateUserCommandHandler(this.UserRepository, null));
            }
        }

        public class The_Method_Handle : CreateUserCommandHandlerTests
        {
            [Fact]
            public async Task Should_verify_if_all_is_called()
            {
                //Act
                await Sut.Handle(this.Command, CancellationToken);

                //Assert
                Mock.Get(this.UserRepository).Verify(x => x.All(), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_commit_is_called()
            {
                //Act
                await Sut.Handle(this.Command, CancellationToken);

                //Assert
                Mock.Get(this.UserRepository).Verify(x => x.UnitOfWork.Commit(), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_updated_is_called()
            {
                //Arrange
                Mock.Get(this.JwtTokenGenerator).Setup(x => x.ValidateAndGenerateTokenAsync(It.IsAny<string>())).ReturnsAsync(new GoogleUserInfo("123", "test@test", "jwt"));

                //Act
                await Sut.Handle(this.Command, CancellationToken);

                //Assert
                Mock.Get(this.UserRepository).Verify(x => x.Update(It.IsAny<User>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_add_is_called()
            {
                //Arrange
                var request = new UserRequest()
                {
                    UserId = "1"
                };
                this.Command = new CreateUserCommand(request);

                //Act
                await Sut.Handle(this.Command, CancellationToken);

                //Assert
                Mock.Get(this.UserRepository).Verify(x => x.Add(It.IsAny<User>()), Times.Once);
            }
        }
    }
}
