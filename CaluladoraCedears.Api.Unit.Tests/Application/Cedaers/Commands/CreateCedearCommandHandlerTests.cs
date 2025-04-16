using CaluladoraCedears.Api.Application.Samples.Commands;
using CaluladoraCedears.Api.Domian;
using CaluladoraCedears.Api.Dto.Samples.Request;
using CaluladoraCedears.Api.Infrastructure.Repositories;
using CaluladoraCedears.Api.Unit.Tests.Base;

using MediatR;

using Moq;

using NetDevPack.Data;

namespace CaluladoraCedears.Api.Unit.Tests.Application.Samples.Commands
{
    public class CreateCedearCommandHandlerTests : BaseTestClass<IRequestHandler<CreateCedearCommand>>
    {
        private readonly ICedearRepository CedearRepository;
        private readonly IUnitOfWork UnitOfWork;

        public CreateCedearCommandHandlerTests()
        {
            this.CedearRepository = Mock.Of<ICedearRepository>();
            this.UnitOfWork = Mock.Of<IUnitOfWork>();

            Mock.Get(this.CedearRepository).Setup(x => x.UnitOfWork).Returns(this.UnitOfWork);

            this.Sut = new CreateCedearCommandHandler(this.CedearRepository);
        }

        public class The_Constructor : CreateCedearCommandHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_sampleRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CreateCedearCommandHandler(null));
            }
        }

        public class The_Method_Handle : CreateCedearCommandHandlerTests
        {
            public The_Method_Handle()
            {
                Mock.Get(this.CedearRepository).Setup(x => x.Add(It.IsAny<Cedear>()));
            }

            [Fact]
            public async Task Should_verify_if_sample_is_added_to_repository()
            {
                //Arrange
                var request = new CreateCedearRequest();
                var command = new CreateCedearCommand(request);

                //Act
                await this.Sut.Handle(command, CancellationToken);

                //Assert
                Mock.Get(this.CedearRepository).Verify(x => x.Add(It.IsAny<Cedear>()), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_sample_is_committed()
            {
                //Arrange
                var request = new CreateCedearRequest();
                var command = new CreateCedearCommand(request);

                //Act
                await this.Sut.Handle(command, CancellationToken);

                //Assert
                Mock.Get(this.CedearRepository.UnitOfWork).Verify(x => x.Commit(), Times.Once);
            }
        }
    }
}