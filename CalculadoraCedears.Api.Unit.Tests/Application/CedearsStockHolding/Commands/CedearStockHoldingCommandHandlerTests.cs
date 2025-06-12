using CalculadoraCedears.Api.Application.CedearsStockHolding.Commands;
using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Dto.CedearsStockHolding.Request;
using CalculadoraCedears.Api.Infrastructure.Repositories;
using CalculadoraCedears.Api.Unit.Tests.Base;

using MediatR;

using MockQueryable;

using Moq;

using NetDevPack.Data;

namespace CalculadoraCedears.Api.Unit.Tests.Application.CedearsStockHolding.Commands
{
    public class CedearStockHoldingCommandHandlerTests : BaseTestClass<IRequestHandler<CedearStockHoldingCommand>>
    {
        private readonly ICedearRepository CedearRepository;
        private readonly ICedearStockHoldingRepository CedearStockHoldingRepository;
        private readonly IBrokerRepository BrokerRepository;
        private readonly IGoogleFinanceRepository GoogleFinanceRepository;
        private readonly IUserRepository UserRepository;
        private readonly IUnitOfWork UnitOfWork;
        private readonly CedearStockHoldingCommand Command; 

        public CedearStockHoldingCommandHandlerTests()
        {
            this.CedearRepository = Mock.Of<ICedearRepository>();
            this.CedearStockHoldingRepository = Mock.Of<ICedearStockHoldingRepository>();
            this.BrokerRepository = Mock.Of<IBrokerRepository>();
            this.GoogleFinanceRepository = Mock.Of<IGoogleFinanceRepository>();
            this.UserRepository = Mock.Of<IUserRepository>();

            this.UnitOfWork = Mock.Of<IUnitOfWork>();

            var cedears = new List<Cedear> { new Cedear("Testing", "TE", "NSYE", 20) };
            var brokers = new List<Broker> { new Broker("Testing", 20) };
            var users = new List<User> { new User("123", "test@test.com") };
            var googleFinance = new GoogleFinance(299M);
            var request = new CedearStockHoldingRequest()
            {
                CedearId = cedears[0].Id,
                BrokerId = brokers[0].Id,
                UserId = "123",
                ExchangeRateCcl = 123,
                PurchasePriceArs = 123,
                Quantity = 1                
            };

            this.Command = new CedearStockHoldingCommand(request);

            Mock.Get(this.UserRepository).Setup(x => x.All()).Returns(users.AsQueryable().BuildMock());
            Mock.Get(this.CedearRepository).Setup(x => x.All()).Returns(cedears.AsQueryable().BuildMock());
            Mock.Get(this.CedearStockHoldingRepository).Setup(x => x.UnitOfWork).Returns(UnitOfWork);
            Mock.Get(this.CedearStockHoldingRepository).Setup(x => x.TryIfAlreadyExistsAsync(It.IsAny<DateTime>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));
            Mock.Get(this.BrokerRepository).Setup(x => x.All()).Returns(brokers.AsQueryable().BuildMock());
            Mock.Get(this.GoogleFinanceRepository).Setup(x => x.TryGetCurrentPriceByTickerAndMarketAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(googleFinance);

            Sut = new CedearStockHoldingCommandHandler(this.CedearStockHoldingRepository, this.CedearRepository, this.BrokerRepository, this.GoogleFinanceRepository, this.UserRepository);
        }

        public class The_Constructor : CedearStockHoldingCommandHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_cedearStockHoldingRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CedearStockHoldingCommandHandler(null, this.CedearRepository, this.BrokerRepository, this.GoogleFinanceRepository, this.UserRepository));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_cedearRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CedearStockHoldingCommandHandler(this.CedearStockHoldingRepository, null, this.BrokerRepository, this.GoogleFinanceRepository, this.UserRepository));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_brokerRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CedearStockHoldingCommandHandler(this.CedearStockHoldingRepository, this.CedearRepository, null, this.GoogleFinanceRepository, this.UserRepository));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_googleFinanceRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CedearStockHoldingCommandHandler(this.CedearStockHoldingRepository, this.CedearRepository, this.BrokerRepository, null, this.UserRepository));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_userRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CedearStockHoldingCommandHandler(this.CedearStockHoldingRepository, this.CedearRepository, this.BrokerRepository, this.GoogleFinanceRepository, null));
            }
        }

        public class The_Method_Handle : CedearStockHoldingCommandHandlerTests
        {
            public The_Method_Handle()
            {
                Mock.Get(this.CedearStockHoldingRepository).Setup(x => x.Add(It.IsAny<Domain.CedearsStockHolding>()));
            }

            [Fact]
            public async Task Should_verify_if_cedearsStockHolding_is_added_to_repository()
            {
                //Act
                await Sut.Handle(this.Command, CancellationToken);

                //Assert
                Mock.Get(this.CedearStockHoldingRepository).Verify(x => x.Add(It.IsAny<Domain.CedearsStockHolding>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_cedearsStockHolding_is_committed()
            {
                //Act
                await Sut.Handle(this.Command, CancellationToken);

                //Assert
                Mock.Get(this.CedearStockHoldingRepository.UnitOfWork).Verify(x => x.Commit(), Times.Once);
            }
        }
    }
}