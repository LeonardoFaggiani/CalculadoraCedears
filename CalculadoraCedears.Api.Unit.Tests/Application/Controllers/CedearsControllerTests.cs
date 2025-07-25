﻿using CalculadoraCedears.Api.Application.Cedears.Queries;
using CalculadoraCedears.Api.Application.CedearsStockHolding.Commands;
using CalculadoraCedears.Api.Application.Controllers;
using CalculadoraCedears.Api.Dto;
using CalculadoraCedears.Api.Dto.Cedears;
using CalculadoraCedears.Api.Unit.Tests.Base;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

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

            Mock.Get(Mediator).Setup(m => m.Send(It.IsAny<CedearsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CedearsQueryResponse(cedaerDtos));
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

        public class The_Method_GetAsync : CedearsControllerTests
        {
            [Fact]
            public async Task Should_verify_if_mediator_was_called()
            {
                //Act                
                await Sut.GetAsync(CancellationToken);

                //Assert
                Mock.Get(Mediator).Verify(x => x.Send(It.IsAny<CedearsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_query_results()
            {
                //Act                
                var result = await Sut.GetAsync(CancellationToken);

                //Assert
                result.As<OkObjectResult>().Value.As<CedearsQueryResponse>().Cedears.First().Name.Should().Be("Tests");
            }
        }
    }
}