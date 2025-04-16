using AutoMapper;

using CaluladoraCedears.Api.Application.Samples.Queries;
using CaluladoraCedears.Api.Domian;
using CaluladoraCedears.Api.Dto;
using CaluladoraCedears.Api.Infrastructure.Repositories;
using CaluladoraCedears.Api.Unit.Tests.Base;

using MockQueryable;

using Moq;

namespace CaluladoraCedears.Api.Unit.Tests.Application.Samples.Queries
{
    public class CedearsQueryHandlerTests : BaseTestClass<CedearsQueryHandler>
    {
        private readonly ICedearRepository CedearRepository;
        private readonly IMapper Mapper;

        public CedearsQueryHandlerTests()
        {
            this.CedearRepository = Mock.Of<ICedearRepository>();
            this.Mapper = Mock.Of<IMapper>();

            var samples = new List<Cedear> { new Cedear("Testing", "TE") };

            Mock.Get(this.CedearRepository).Setup(x => x.All()).Returns(samples.AsQueryable().BuildMock());
            Mock.Get(this.Mapper).Setup(x => x.Map<IEnumerable<Cedear>, IEnumerable<CedaerDto>>(It.IsAny<IEnumerable<Cedear>>())).Returns(new List<CedaerDto> { new CedaerDto() { Id = Guid.NewGuid(), Description = "Testing" } });

            this.Sut = new CedearsQueryHandler(this.CedearRepository, this.Mapper);
        }

        public class The_Constructor : CedearsQueryHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_sampleRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CedearsQueryHandler(null, this.Mapper));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_mapper_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new CedearsQueryHandler(this.CedearRepository, null));
            }
        }

        public class The_Method_Handle : CedearsQueryHandlerTests
        {
            public The_Method_Handle()
            {
                Mock.Get(this.CedearRepository).Setup(x => x.Add(It.IsAny<Cedear>()));
            }

            [Fact]
            public async Task Should_verify_if_all_service_is_called()
            {
                //Arrange
                var query = new CedearsQuery();

                //Act
                await this.Sut.Handle(query, CancellationToken);

                //Assert
                Mock.Get(this.CedearRepository).Verify(x => x.All(), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_map_sample_to_sampleByIdQueryResponse_is_called()
            {
                //Arrange
                var query = new CedearsQuery();

                //Act
                await this.Sut.Handle(query, CancellationToken);

                //Assert
                Mock.Get(this.Mapper).Verify(x => x.Map<IEnumerable<Cedear>, IEnumerable<CedaerDto>>(It.IsAny<IEnumerable<Cedear>>()), Times.Once);
            }
        }
    }
}