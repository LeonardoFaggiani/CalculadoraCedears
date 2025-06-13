using CalculadoraCedears.Api.Infrastructure.Data;
using CalculadoraCedears.Api.Infrastructure.Repositories.Base;
using CalculadoraCedears.Api.Unit.Tests.Base;

using FluentAssertions;

using NetDevPack.Domain;

namespace CalculadoraCedears.Api.Unit.Tests.Infrastructure.Repositories.Base
{
    public class EntityTestClass : Entity
    { }

    public class RepositoryTests : BaseTestClass<Repository<EntityTestClass>>
    {
        public CalculadoraCedearsContext DbContext { get; set; }
        public RepositoryTests()
        {

            DbContext = MockDbContext.Of<CalculadoraCedearsContext>();
            Sut = new Repository<EntityTestClass>(DbContext);
        }

        public class TheConstructor : RepositoryTests
        {
            [Fact]
            public void Should_throw_ArgumentNullException_when_dbContext_is_null()
            {
                // Arrange
                DbContext = null;

                // Act & Assert
                Assert.Throws<ArgumentNullException>("context", () => new Repository<EntityTestClass>(DbContext));
            }
        }

        public class TheMethod_All : RepositoryTests
        {
            [Fact]
            public void Should_return_an_queryable_from_the_entity()
            {
                // Arrange
                var repository = new List<EntityTestClass>() { };
                Mock.Get(DbContext).SetupDbSet(repository);

                // Act
                var result = Sut.All();

                // Assert
                result.Should().BeAssignableTo<IQueryable<EntityTestClass>>();
            }
        }

        public class TheMethod_Update : RepositoryTests
        {
            [Fact]
            public void Should_update_entity()
            {
                // Arrange
                var entityToUpdate = new EntityTestClass { Id = Guid.NewGuid() };

                // Act
                Sut.Update(entityToUpdate);

                // Assert
                Mock.Get(DbContext).Verify(x => x.Update(It.IsAny<EntityTestClass>()), Times.Once);
            }
        }

        public class TheMethod_Add : RepositoryTests
        {
            [Fact]
            public void Should_insert_entity()
            {
                // Arrange
                var entityToInsert = new EntityTestClass { Id = Guid.NewGuid() };

                // Act
                Sut.Add(entityToInsert);

                // Assert
                Mock.Get(DbContext).Verify(x => x.Add(It.IsAny<EntityTestClass>()), Times.Once);
            }
        }
    }
}
