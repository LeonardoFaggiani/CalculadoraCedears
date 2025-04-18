using CalculadoraCedears.Api.Application.Cedears.Commands;
using CalculadoraCedears.Api.Domian;
using CalculadoraCedears.Api.Dto.Cedears.Request;
using CalculadoraCedears.Api.Infrastructure.Repositories;
using CalculadoraCedears.Api.Unit.Tests.Base;

using MediatR;

using Moq;

using NetDevPack.Data;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Cedaers.Commands
{
    public class CreateCedearCommandHandlerTests : BaseTestClass<IRequestHandler<CreateCedearCommand>>
    {
        private readonly ICedearRepository CedearRepository;
        private readonly IUnitOfWork UnitOfWork;

        public CreateCedearCommandHandlerTests()
        {
            CedearRepository = Mock.Of<ICedearRepository>();
            UnitOfWork = Mock.Of<IUnitOfWork>();

            Mock.Get(CedearRepository).Setup(x => x.UnitOfWork).Returns(UnitOfWork);

            Sut = new CreateCedearCommandHandler(CedearRepository);
        }

        public class The_Constructor : CreateCedearCommandHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_cedearRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CreateCedearCommandHandler(null));
            }
        }

        public class The_Method_Handle : CreateCedearCommandHandlerTests
        {
            public The_Method_Handle()
            {
                Mock.Get(CedearRepository).Setup(x => x.Add(It.IsAny<Cedear>()));
            }

            [Fact]
            public async Task Should_verify_if_cedear_is_added_to_repository()
            {
                //Arrange
                var request = new CreateCedearRequest();
                var command = new CreateCedearCommand(request);

                //Act
                await Sut.Handle(command, CancellationToken);

                //Assert
                Mock.Get(CedearRepository).Verify(x => x.Add(It.IsAny<Cedear>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_cedear_is_committed()
            {
                //Arrange
                var request = new CreateCedearRequest();
                var command = new CreateCedearCommand(request);

                //Act
                await Sut.Handle(command, CancellationToken);

                //Assert
                Mock.Get(CedearRepository.UnitOfWork).Verify(x => x.Commit(), Times.Once);
            }
        }
    }
}