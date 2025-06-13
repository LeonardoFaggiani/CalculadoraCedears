using AutoMapper;

using CalculadoraCedears.Api.Application.CedearsStockHolding.Queries;
using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Dto;
using CalculadoraCedears.Api.Infrastructure.Repositories;
using CalculadoraCedears.Api.Unit.Tests.Base;

namespace CalculadoraCedears.Api.Unit.Tests.Application.CedearsStockHolding.Queries
{
    public class CedearsStockHoldingQueryHandlerTests : BaseTestClass<CedearsStockHoldingQueryHandler>
    {
        private readonly ICedearStockHoldingRepository CedearStockHoldingRepository;
        private readonly IMapper Mapper;

        public CedearsStockHoldingQueryHandlerTests()
        {
            this.CedearStockHoldingRepository = Mock.Of<ICedearStockHoldingRepository>();
            this.Mapper = Mock.Of<IMapper>();

            var cedearsStockHolding = new Domain.CedearsStockHolding(1, DateTime.Now, 200M, 200M);
            var cedear = new Cedear("BBD", "BBD", "DD", 2);
            cedear.SetPrice(213);

            cedearsStockHolding.SetCedear(cedear);

            var cedearsStackHoldings = new List<Domain.CedearsStockHolding>()
            {
               cedearsStockHolding
            };

            var keyValuePairs = new Dictionary<string, List<Domain.CedearsStockHolding>>
            {
                { "BBD", cedearsStackHoldings }
            };

            var cedearStockHoldingDto = new CedearStockHoldingDto();

            Mock.Get(this.CedearStockHoldingRepository).Setup(x => x.GetActivesAndGroupedByTickerAsync(It.IsAny<string>(), It.IsAny<CancellationToken>(), default)).ReturnsAsync(keyValuePairs);
            Mock.Get(this.Mapper).Setup(x => x.Map<Domain.CedearsStockHolding, CedearStockHoldingDto>(It.IsAny<Domain.CedearsStockHolding>())).Returns(cedearStockHoldingDto);

            Sut = new CedearsStockHoldingQueryHandler(this.CedearStockHoldingRepository, this.Mapper);
        }

        public class The_Constructor : CedearsStockHoldingQueryHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_cedearStockHoldingRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CedearsStockHoldingQueryHandler(null, Mapper));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_mapper_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CedearsStockHoldingQueryHandler(this.CedearStockHoldingRepository, null));
            }
        }

        public class The_Method_Handle : CedearsStockHoldingQueryHandlerTests
        {
            private CedearsStockHoldingQuery CedearsStockHoldingQuery;
            public The_Method_Handle()
            {
                this.CedearsStockHoldingQuery = new CedearsStockHoldingQuery("123");
            }

            [Fact]
            public async Task Should_verify_if_all_service_is_called()
            {
                //Act
                await Sut.Handle(this.CedearsStockHoldingQuery, CancellationToken);

                //Assert
                Mock.Get(this.CedearStockHoldingRepository).Verify(x => x.GetActivesAndGroupedByTickerAsync(It.IsAny<string>(), It.IsAny<CancellationToken>(), default), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_mapper_is_called()
            {
                //Act
                await Sut.Handle(this.CedearsStockHoldingQuery, CancellationToken);

                //Assert
                Mock.Get(this.Mapper).Verify(x => x.Map<Domain.CedearsStockHolding, CedearStockHoldingDto>(It.IsAny<Domain.CedearsStockHolding>()), Times.Once);
            }
        }
    }
}