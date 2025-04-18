using CalculadoraCedears.Api.Application.Cedears.Commands;
using CalculadoraCedears.Api.Domian;
using CalculadoraCedears.Api.Dto.Cedears.Request;
using CalculadoraCedears.Api.Infrastructure.Repositories;
using CalculadoraCedears.Api.Unit.Tests.Base;

using MediatR;

using MockQueryable;

using Moq;

using NetDevPack.Data;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Cedaers.Commands
{
    public class CreateCedearCommandHandlerTests : BaseTestClass<IRequestHandler<CedearStockHoldingCommand>>
    {
        private readonly ICedearRepository CedearRepository;
        private readonly ICedearStockHoldingRepository CedearStockHoldingRepository;
        private readonly IBrokerRepository BrokerRepository;
        private readonly IUnitOfWork UnitOfWork;

        public CreateCedearCommandHandlerTests()
        {
            this.CedearRepository = Mock.Of<ICedearRepository>();
            this.CedearStockHoldingRepository = Mock.Of<ICedearStockHoldingRepository>();
            this.BrokerRepository = Mock.Of<IBrokerRepository>();
            this.UnitOfWork = Mock.Of<IUnitOfWork>();

            var cedears = new List<Cedear> { new Cedear("Testing", "TE", "NSYE", 1) };
            var brokers = new List<Broker> { new Broker("Testing", 1) };

            Mock.Get(this.CedearStockHoldingRepository).Setup(x => x.UnitOfWork).Returns(UnitOfWork);
            Mock.Get(this.CedearStockHoldingRepository).Setup(x => x.VerifyIfAlreadyExistsAsync(It.IsAny<DateTime>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

            Mock.Get(this.CedearRepository).Setup(x => x.All()).Returns(cedears.AsQueryable().BuildMock());
            Mock.Get(this.BrokerRepository).Setup(x => x.All()).Returns(brokers.AsQueryable().BuildMock());

            Sut = new CedearStockHoldingCommandHandler(this.CedearStockHoldingRepository, this.CedearRepository, this.BrokerRepository);
        }

        public class The_Constructor : CreateCedearCommandHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_cedearStockHoldingRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CedearStockHoldingCommandHandler(null, this.CedearRepository, this.BrokerRepository));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_cedearRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CedearStockHoldingCommandHandler(this.CedearStockHoldingRepository, null, this.BrokerRepository));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_brokerRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CedearStockHoldingCommandHandler(this.CedearStockHoldingRepository, this.CedearRepository, null));
            }
        }

        public class The_Method_Handle : CreateCedearCommandHandlerTests
        {
            public The_Method_Handle()
            {
                Mock.Get(this.CedearStockHoldingRepository).Setup(x => x.Add(It.IsAny<CedearsStockHolding>()));
            }

            [Fact]
            public async Task Should_verify_if_cedearsStockHolding_is_added_to_repository()
            {
                //Arrange
                var request = new CedearStockHoldingRequest();
                var command = new CedearStockHoldingCommand(request);

                //Act
                await Sut.Handle(command, CancellationToken);

                //Assert
                Mock.Get(this.CedearStockHoldingRepository).Verify(x => x.Add(It.IsAny<CedearsStockHolding>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_cedearsStockHolding_is_committed()
            {
                //Arrange
                var request = new CedearStockHoldingRequest();
                var command = new CedearStockHoldingCommand(request);

                //Act
                await Sut.Handle(command, CancellationToken);

                //Assert
                Mock.Get(this.CedearStockHoldingRepository.UnitOfWork).Verify(x => x.Commit(), Times.Once);
            }
        }
    }
}