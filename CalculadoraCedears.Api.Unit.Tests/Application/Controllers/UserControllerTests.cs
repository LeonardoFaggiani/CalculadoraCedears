using CalculadoraCedears.Api.Application.Controllers;
using CalculadoraCedears.Api.Application.Users.Commands;
using CalculadoraCedears.Api.Dto.Users.Request;

using FluentAssertions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Controllers
{
    public class UserControllerTests
    {
        private readonly UserController Sut;
        private readonly IMediator Mediator;
        private CancellationToken CancellationToken = CancellationToken.None;

        public UserControllerTests()
        {
            this.Mediator = Mock.Of<IMediator>();
            this.Sut = new UserController(this.Mediator);
        }

        public class The_Constructor : UserControllerTests
        {
            [Fact]
            public void Should_throws_argumentNullException_when_mediator_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new UserController(null));
            }
        }

        public class The_Method_GetAsync : UserControllerTests
        {
            [Fact]
            public async void Should_call_service_postLoginAsync()
            {
                var request = new UserRequest();

                var result = await this.Sut.PostLoginAsync(request, this.CancellationToken);

                Mock.Get(this.Mediator).Verify(x => x.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        public class The_Method_PostAuthSuccessAsync : UserControllerTests
        {
            [Fact]
            public async void Should_call_service_postAuthSuccessAsync()
            {
                var result = await this.Sut.PostAuthSuccessAsync(this.CancellationToken);

                result.As<RedirectResult>();
            }
        }
    }
}
