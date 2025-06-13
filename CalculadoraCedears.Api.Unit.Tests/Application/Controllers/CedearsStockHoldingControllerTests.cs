using CalculadoraCedears.Api.Application.CedearsStockHolding.Commands;
using CalculadoraCedears.Api.Application.CedearsStockHolding.Queries;
using CalculadoraCedears.Api.Application.Controllers;
using CalculadoraCedears.Api.Dto.CedearsStockHolding.Request;

using MediatR;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Controllers
{
    public class CedearsStockHoldingControllerTests
    {
        private readonly CedearsStockHoldingController Sut;
        private readonly IMediator Mediator;
        private CancellationToken CancellationToken = CancellationToken.None;

        public CedearsStockHoldingControllerTests()
        {
            this.Mediator = Mock.Of<IMediator>();
            this.Sut = new CedearsStockHoldingController(this.Mediator);
        }


        public class The_Constructor : CedearsStockHoldingControllerTests
        {
            [Fact]
            public void Should_throws_argumentNullException_when_mediator_is_null()
            {
                Assert.Throws<ArgumentNullException>(() => new CedearsStockHoldingController(null));
            }
        }

        public class The_Method_GetAsync : CedearsStockHoldingControllerTests
        {
            [Fact]
            public async void Should_call_service_getAsync()
            {
                var result = await this.Sut.GetAsync("123", this.CancellationToken);

                Mock.Get(this.Mediator).Verify(x => x.Send(It.IsAny<CedearsStockHoldingQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        public class The_Method_PostAsync : CedearsStockHoldingControllerTests
        {
            [Fact]
            public async void Should_call_service_postAsync()
            {
                var request = new CedearStockHoldingRequest();

                var result = await this.Sut.PostAsync(request, this.CancellationToken);

                Mock.Get(this.Mediator).Verify(x => x.Send(It.IsAny<CedearStockHoldingCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        public class The_Method_PutAsync : CedearsStockHoldingControllerTests
        {
            [Fact]
            public async void Should_call_service_putAsync()
            {
                var request = new UpdateCedearStockHoldingRequest();

                var result = await this.Sut.PutAsync(request, this.CancellationToken);

                Mock.Get(this.Mediator).Verify(x => x.Send(It.IsAny<UpdateCedearStockHoldingCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        public class The_Method_DeleteAsync : CedearsStockHoldingControllerTests
        {
            [Fact]
            public async void Should_call_service_deleteAsync()
            {
                var result = await this.Sut.DeleteAsync(Guid.NewGuid(), this.CancellationToken);

                Mock.Get(this.Mediator).Verify(x => x.Send(It.IsAny<DeleteCedearStockHoldingCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            }
        }
    }
}