﻿using AutoMapper;

using CalculadoraCedears.Api.Application.Cedears.Queries;
using CalculadoraCedears.Api.Domian;
using CalculadoraCedears.Api.Dto;
using CalculadoraCedears.Api.Infrastructure.Repositories;
using CalculadoraCedears.Api.Unit.Tests.Base;

using MockQueryable;

using Moq;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Cedaers.Queries
{
    public class CedearsQueryHandlerTests : BaseTestClass<SearchCedearsByTickerQueryHandler>
    {
        private readonly ICedearRepository CedearRepository;
        private readonly IMapper Mapper;

        public CedearsQueryHandlerTests()
        {
            CedearRepository = Mock.Of<ICedearRepository>();
            Mapper = Mock.Of<IMapper>();

            var cedears = new List<Cedear> { new Cedear("Testing", "TE", "NSYE", 1) };

            Mock.Get(CedearRepository).Setup(x => x.All()).Returns(cedears.AsQueryable().BuildMock());
            Mock.Get(Mapper).Setup(x => x.Map<IEnumerable<Cedear>, IEnumerable<CedaerDto>>(It.IsAny<IEnumerable<Cedear>>())).Returns(new List<CedaerDto> { new CedaerDto() { Id = Guid.NewGuid(), Name = "Testing", Ticker = "TE" } });

            Sut = new SearchCedearsByTickerQueryHandler(CedearRepository, Mapper);
        }

        public class The_Constructor : CedearsQueryHandlerTests
        {
            [Fact]
            public void Should_throw_an_ArgumentNullException_when_cedearRepository_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new SearchCedearsByTickerQueryHandler(null, Mapper));
            }

            [Fact]
            public void Should_throw_an_ArgumentNullException_when_mapper_is_null()
            {
                //Act & Assert
                Assert.Throws<ArgumentNullException>(() => new SearchCedearsByTickerQueryHandler(CedearRepository, null));
            }
        }

        public class The_Method_Handle : CedearsQueryHandlerTests
        {
            private SearchCedearsByTickerQuery SearchCedearsByTickerQuery;
            public The_Method_Handle()
            {
                this.SearchCedearsByTickerQuery = new SearchCedearsByTickerQuery
                {
                    Ticker = "TE"
                };

                Mock.Get(CedearRepository).Setup(x => x.Add(It.IsAny<Cedear>()));
            }

            [Fact]
            public async Task Should_verify_if_all_service_is_called()
            {
                //Act
                await Sut.Handle(this.SearchCedearsByTickerQuery, CancellationToken);

                //Assert
                Mock.Get(CedearRepository).Verify(x => x.All(), Times.Once);
            }

            [Fact]
            public async Task Should_verify_if_map_cedear_to_cedaerDto_is_called()
            {
                //Act
                await Sut.Handle(this.SearchCedearsByTickerQuery, CancellationToken);

                //Assert
                Mock.Get(Mapper).Verify(x => x.Map<IEnumerable<Cedear>, IEnumerable<CedaerDto>>(It.IsAny<IEnumerable<Cedear>>()), Times.Once);
            }
        }
    }
}