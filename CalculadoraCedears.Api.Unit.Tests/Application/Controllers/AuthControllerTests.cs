using CalculadoraCedears.Api.Application.Controllers;
using CalculadoraCedears.Api.Application.Auth.Commands;

using FluentAssertions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController Sut;
        private readonly IMediator Mediator;
        private CancellationToken CancellationToken = CancellationToken.None;

        public AuthControllerTests()
        {
            this.Mediator = Mock.Of<IMediator>();
            this.Sut = new AuthController(this.Mediator);
        }

        public class The_Constructor : AuthControllerTests
        {
            [Fact]
            public void Should_throws_argumentNullException_when_mediator_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new AuthController(null));
            }
        }

        public class The_Method_PostAuthSuccessAsync : AuthControllerTests
        {
            public The_Method_PostAuthSuccessAsync()
            {
                Mock.Get(this.Mediator).Setup(x => x.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Dto.Auth.Response.CreateUserCommandResponse("jwt", "refresh"));
            }

            [Fact]
            public async Task Should_call_createUserCommand()
            {
                await this.Sut.PostAuthSuccessAsync("jwtTest", "state", this.CancellationToken);

                Mock.Get(this.Mediator).Verify(x => x.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task Should_check_result_redirect()
            {
                var result = await this.Sut.PostAuthSuccessAsync("jwtTest", "state", this.CancellationToken);

                result.As<RedirectResult>();
            }
        }
    }
}
