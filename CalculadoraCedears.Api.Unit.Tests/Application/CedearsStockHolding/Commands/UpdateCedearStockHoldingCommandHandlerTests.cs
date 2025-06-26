using AutoMapper;

using CalculadoraCedears.Api.Application.CedearsStockHolding.Commands;
using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Dto.CedearsStockHolding.Request;
using CalculadoraCedears.Api.Infrastructure.Repositories;
using CalculadoraCedears.Api.Unit.Tests.Base;

using MediatR;

using MockQueryable;

using NetDevPack.Data;

namespace CalculadoraCedears.Api.Unit.Tests.Application.CedearsStockHolding.Commands
{
    public class UpdateCedearStockHoldingCommandHandlerTests : BaseTestClass<IRequestHandler<UpdateCedearStockHoldingCommand>>
    {
        private readonly ICedearStockHoldingRepository CedearStockHoldingRepository;
        private readonly IGoogleRepository GoogleRepository;
        private readonly IMapper Mapper;

        public UpdateCedearStockHoldingCommandHandlerTests()
        {
            this.CedearStockHoldingRepository = Mock.Of<ICedearStockHoldingRepository>(x => x.UnitOfWork == Mock.Of<IUnitOfWork>());
            this.GoogleRepository = Mock.Of<IGoogleRepository>();
            this.Mapper = Mock.Of<IMapper>();

            Sut = new UpdateCedearStockHoldingCommandHandler(this.CedearStockHoldingRepository, this.GoogleRepository, this.Mapper);
        }

        public class The_Constructor : UpdateCedearStockHoldingCommandHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_cedearStockHoldingRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new UpdateCedearStockHoldingCommandHandler(null, this.GoogleRepository, this.Mapper));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_googleRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new UpdateCedearStockHoldingCommandHandler(this.CedearStockHoldingRepository, null, this.Mapper));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_mapper_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new UpdateCedearStockHoldingCommandHandler(this.CedearStockHoldingRepository, this.GoogleRepository, null));
            }
        }

        public class The_Method_Handle : UpdateCedearStockHoldingCommandHandlerTests
        {
            private readonly UpdateCedearStockHoldingCommand Command;
            public The_Method_Handle()
            {
                var cedearsStockHolding = new Domain.CedearsStockHolding(1, DateTime.Now, 233M, 233M);
                cedearsStockHolding.SetCedear(new Cedear("TEST", "TEST", "NASDAQ", 2));
                cedearsStockHolding.SetBroker(new Broker("TEST", 2));

                var cedearsStockHoldings = new List<Domain.CedearsStockHolding> {
                    cedearsStockHolding
                };
                var request = new UpdateCedearStockHoldingRequest();
                request.Id = cedearsStockHoldings[0].Id;
                var googleFinance = new GoogleFinance(299M);

                Command = new UpdateCedearStockHoldingCommand(request);

                Mock.Get(this.Mapper).Setup(x => x.Map(It.IsAny<UpdateCedearStockHoldingRequest>(), It.IsAny<Domain.CedearsStockHolding>())).Returns(cedearsStockHoldings[0]);
                Mock.Get(this.CedearStockHoldingRepository).Setup(x => x.All()).Returns(cedearsStockHoldings.AsQueryable().BuildMock());
                Mock.Get(this.GoogleRepository).Setup(x => x.TryGetFromFinanceCurrentPriceByTickerAndMarketAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(googleFinance);
            }

            [Fact]
            public async Task Should_verify_if_cedearsStockHolding_is_commited_to_repository()
            {
                //Act
                await Sut.Handle(this.Command, CancellationToken);

                //Assert
                Mock.Get(this.CedearStockHoldingRepository).Verify(x => x.UnitOfWork.Commit(), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_cedearsStockHolding_is_update_to_repository()
            {
                //Act
                await Sut.Handle(this.Command, CancellationToken);

                //Assert
                Mock.Get(this.CedearStockHoldingRepository).Verify(x => x.Update(It.IsAny<Domain.CedearsStockHolding>()), Times.Once);
            }
        }
    }
}
