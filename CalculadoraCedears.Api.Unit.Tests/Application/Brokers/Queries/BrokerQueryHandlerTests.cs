using AutoMapper;

using CalculadoraCedears.Api.Application.Brokers.Queries;
using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Dto;
using CalculadoraCedears.Api.Infrastructure.Repositories;
using CalculadoraCedears.Api.Unit.Tests.Base;

using MockQueryable;

using Moq;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Brokers.Queries
{
    public class BrokerQueryHandlerTests : BaseTestClass<BrokerQueryHandler>
    {
        private readonly IBrokerRepository BrokerRepository;
        private readonly IMapper Mapper;

        public BrokerQueryHandlerTests()
        {
            this.BrokerRepository = Mock.Of<IBrokerRepository>();
            this.Mapper = Mock.Of<IMapper>();

            var brokers = new List<Broker> { new Broker("Testing", 1) };

            Mock.Get(this.BrokerRepository).Setup(x => x.All()).Returns(brokers.AsQueryable().BuildMock());
            Mock.Get(this.Mapper).Setup(x => x.Map<IEnumerable<Broker>, IEnumerable<BrokerDto>>(It.IsAny<IEnumerable<Broker>>())).Returns(new List<BrokerDto> { new BrokerDto() { Id = 1, Name = "Testing" } });

            Sut = new BrokerQueryHandler(this.BrokerRepository, this.Mapper);
        }

        public class The_Constructor : BrokerQueryHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_brokerRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new BrokerQueryHandler(null, Mapper));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_mapper_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new BrokerQueryHandler(this.BrokerRepository, null));
            }
        }

        public class The_Method_Handle : BrokerQueryHandlerTests
        {
            private BrokerQuery BrokerQuery;
            public The_Method_Handle()
            {
                BrokerQuery = new BrokerQuery();
            }

            [Fact]
            public async Task Should_verify_if_all_service_is_called()
            {
                //Act
                await Sut.Handle(this.BrokerQuery, CancellationToken);

                //Assert
                Mock.Get(this.BrokerRepository).Verify(x => x.All(), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_map_broker_to_brokerDto_is_called()
            {
                //Act
                await Sut.Handle(this.BrokerQuery, CancellationToken);

                //Assert
                Mock.Get(Mapper).Verify(x => x.Map<IEnumerable<Broker>, IEnumerable<BrokerDto>>(It.IsAny<IEnumerable<Broker>>()), Times.Once);
            }
        }
    }
}
