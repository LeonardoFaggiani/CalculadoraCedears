using CalculadoraCedears.Api.Application.CedearsStockHolding.Commands;
using CalculadoraCedears.Api.Application.Controllers;
using CalculadoraCedears.Api.Dto;
using CalculadoraCedears.Api.Dto.CedearsStockHolding.Request;
using CalculadoraCedears.Api.Unit.Tests.Base;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using Moq;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Controllers
{
    public class CedearsStockHoldingControllerTests : BaseTestClass<CedearsStockHoldingController>
    {
        public CedearsStockHoldingControllerTests()
        {
            var cedaerDtos = new List<CedearDto>()
            {
                new CedearDto()
                {
                    Id = new Guid("5e832cf5-6934-485f-bd8a-207eb3eadc4c"),
                    Name = "Tests"
                }
            };

            Mock.Get(Mediator).Setup(m => m.Send(It.IsAny<CedearStockHoldingCommand>(), It.IsAny<CancellationToken>()));

            Sut = new CedearsStockHoldingController(Mediator);
        }

        public class The_Constructor : CedearsStockHoldingControllerTests
        {
            [Fact]
            public void Should_throw_an_argumentNullException_when_mediator_is_null()
            {
                // act & assert
                Assert.Throws<ArgumentNullException>(() => new CedearsStockHoldingController(null));
            }
        }
        

        public class The_Method_PostAsync : CedearsStockHoldingControllerTests
        {
            private CedearStockHoldingRequest Request;

            public The_Method_PostAsync()
            {
                Request = new CedearStockHoldingRequest();
            }

            [Fact]
            public async Task Should_verify_if_mediator_was_called()
            {
                //Act                
                await Sut.PostAsync(Request, CancellationToken);

                //Assert
                Mock.Get(Mediator).Verify(x => x.Send(It.IsAny<CedearStockHoldingCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_query_results()
            {
                //Act                
                var result = await Sut.PostAsync(Request, CancellationToken);

                //Assert
                result.As<OkObjectResult>().Value.As<IActionResult>();
            }
        }
    }
}