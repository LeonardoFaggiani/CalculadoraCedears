using CaluladoraCedears.Api.Application.Controllers;
using CaluladoraCedears.Api.Application.Samples.Commands;
using CaluladoraCedears.Api.Application.Samples.Queries;
using CaluladoraCedears.Api.Dto;
using CaluladoraCedears.Api.Dto.Samples;
using CaluladoraCedears.Api.Dto.Samples.Request;
using CaluladoraCedears.Api.Unit.Tests.Base;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using Moq;

namespace CaluladoraCedears.Api.Unit.Tests.Application.Controllers
{
    public class CedearsControllerTests : BaseTestClass<CedearsController>
    {
        public CedearsControllerTests()
        {
            var samplesDto = new List<CedaerDto>()
            {
                new CedaerDto()
                {
                    Id = new Guid("5e832cf5-6934-485f-bd8a-207eb3eadc4c"),
                    Description = "Tests"
                }
            };

            Mock.Get(Mediator).Setup(m => m.Send(It.IsAny<CedearsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new CedearsQueryResponse(samplesDto));
            Mock.Get(Mediator).Setup(m => m.Send(It.IsAny<CreateCedearCommand>(), It.IsAny<CancellationToken>()));

            this.Sut = new CedearsController(Mediator);
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
                await this.Sut.GetAsync(CancellationToken);

                //Assert
                Mock.Get(Mediator).Verify(x => x.Send(It.IsAny<CedearsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_query_results()
            {
                //Act                
                var result = await this.Sut.GetAsync(CancellationToken);

                //Assert
                result.As<OkObjectResult>().Value.As<CedearsQueryResponse>().Samples.First().Description.Should().Be("Tests");
            }
        }     

        public class The_Method_PostCedearsAsync : CedearsControllerTests
        {
            private CreateCedearRequest Request;

            public The_Method_PostCedearsAsync()
            {
                this.Request = new CreateCedearRequest()
                {
                    Description = "Tests"
                };
            }

            [Fact]
            public async Task Should_verify_if_mediator_was_called()
            {
                //Act                
                await this.Sut.PostCedearAsync(this.Request, CancellationToken);

                //Assert
                Mock.Get(Mediator).Verify(x => x.Send(It.IsAny<CreateCedearCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_query_results()
            {
                //Act                
                var result = await this.Sut.PostCedearAsync(this.Request, CancellationToken);

                //Assert
                result.As<OkObjectResult>().Value.As<IActionResult>();
            }
        }
    }
}