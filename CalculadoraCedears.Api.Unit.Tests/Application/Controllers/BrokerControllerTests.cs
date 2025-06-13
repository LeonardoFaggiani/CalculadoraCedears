using CalculadoraCedears.Api.Application.Brokers.Queries;
using CalculadoraCedears.Api.Application.Controllers;

using MediatR;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Controllers
{
    public class BrokerControllerTests
    {
        private readonly BrokerController Sut;
        private readonly IMediator Mediator;
        private CancellationToken CancellationToken = CancellationToken.None;

        public BrokerControllerTests()
        {
            this.Mediator = Mock.Of<IMediator>();
            this.Sut = new BrokerController(this.Mediator);
        }

        public class The_Constructor : BrokerControllerTests
        {
            [Fact]
            public void Should_throws_argumentNullException_when_mediator_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new BrokerController(null));
            }
        }

        public class The_Method_GetAsync : BrokerControllerTests
        {
            [Fact]
            public async void Should_call_service_getAsync()
            {
                var result = await this.Sut.GetAsync(this.CancellationToken);

                Mock.Get(this.Mediator).Verify(x => x.Send(It.IsAny<BrokerQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            }
        }
    }
}
