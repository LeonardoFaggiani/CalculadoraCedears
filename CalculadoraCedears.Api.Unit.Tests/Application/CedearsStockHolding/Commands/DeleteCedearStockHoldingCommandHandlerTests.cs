using CalculadoraCedears.Api.Application.CedearsStockHolding.Commands;
using CalculadoraCedears.Api.Infrastructure.Repositories;
using CalculadoraCedears.Api.Unit.Tests.Base;

using MediatR;

using MockQueryable;

using Moq;

using NetDevPack.Data;

namespace CalculadoraCedears.Api.Unit.Tests.Application.CedearsStockHolding.Commands
{
    public class DeleteCedearStockHoldingCommandHandlerTests : BaseTestClass<IRequestHandler<DeleteCedearStockHoldingCommand>>
    {
        private readonly ICedearStockHoldingRepository CedearStockHoldingRepository;
        public DeleteCedearStockHoldingCommandHandlerTests()
        {
            this.CedearStockHoldingRepository = Mock.Of<ICedearStockHoldingRepository>(x => x.UnitOfWork == Mock.Of<IUnitOfWork>());

            Sut = new DeleteCedearStockHoldingCommandHandler(this.CedearStockHoldingRepository);
        }

        public class The_Constructor : DeleteCedearStockHoldingCommandHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_cedearStockHoldingRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new DeleteCedearStockHoldingCommandHandler(null));
            }
        }

        public class The_Method_Handle : DeleteCedearStockHoldingCommandHandlerTests
        {
            private readonly DeleteCedearStockHoldingCommand Command;
            public The_Method_Handle()
            {
                var cedearsStockHolding = new Domain.CedearsStockHolding(1, DateTime.Now, 123, 123);
                var cedearsStockHoldings = new List<Domain.CedearsStockHolding> { cedearsStockHolding };
                Command = new DeleteCedearStockHoldingCommand(cedearsStockHolding.Id);

                Mock.Get(this.CedearStockHoldingRepository).Setup(x => x.All()).Returns(cedearsStockHoldings.AsQueryable().BuildMock());
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
            public async Task Should_verify_if_cedearsStockHolding_is_delete_to_repository()
            {
                //Act
                await Sut.Handle(this.Command, CancellationToken);

                //Assert
                Mock.Get(this.CedearStockHoldingRepository).Verify(x => x.Delete(It.IsAny<Domain.CedearsStockHolding>()), Times.Once);
            }
        }
    }
}