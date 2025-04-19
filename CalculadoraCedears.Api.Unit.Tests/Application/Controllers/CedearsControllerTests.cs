using CalculadoraCedears.Api.Application.Cedears.Queries;
using CalculadoraCedears.Api.Application.CedearsStockHolding.Commands;
using CalculadoraCedears.Api.Application.Controllers;
using CalculadoraCedears.Api.Dto;
using CalculadoraCedears.Api.Dto.Cedears;
using CalculadoraCedears.Api.Unit.Tests.Base;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using Moq;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Controllers
{
    public class CedearsControllerTests : BaseTestClass<CedearsController>
    {
        public CedearsControllerTests()
        {
            var cedaerDtos = new List<CedearDto>()
            {
                new CedearDto()
                {
                    Id = new Guid("5e832cf5-6934-485f-bd8a-207eb3eadc4c"),
                    Name = "Tests"
                }
            };

            Mock.Get(Mediator).Setup(m => m.Send(It.IsAny<SearchCedearsByTickerQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SearchCedearsByTickerQueryResponse(cedaerDtos));
            Mock.Get(Mediator).Setup(m => m.Send(It.IsAny<CedearStockHoldingCommand>(), It.IsAny<CancellationToken>()));

            Sut = new CedearsController(Mediator);
        }

        public class The_Constructor : CedearsControllerTests
        {
            [Fact]
            public void Should_throw_an_argumentNullException_when_mediator_is_null()
            {
                // act & assert
                Assert.Throws<ArgumentNullException>(() => new CedearsController(null));
            }
        }

        public class The_Method_GetSearchByTickerAsync : CedearsControllerTests
        {
            private SearchCedearsByTickerQuery Query;
            public The_Method_GetSearchByTickerAsync()
            {
                this.Query = new SearchCedearsByTickerQuery();
            }

            [Fact]
            public async Task Should_verify_if_mediator_was_called()
            {
                //Act                
                await Sut.GetSearchByTickerAsync(this.Query, CancellationToken);

                //Assert
                Mock.Get(Mediator).Verify(x => x.Send(It.IsAny<SearchCedearsByTickerQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_query_results()
            {
                //Act                
                var result = await Sut.GetSearchByTickerAsync(this.Query, CancellationToken);

                //Assert
                result.As<OkObjectResult>().Value.As<SearchCedearsByTickerQueryResponse>().Cedears.First().Name.Should().Be("Tests");
            }
        }
    }
}