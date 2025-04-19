using CalculadoraCedears.Api.Application.Cedears.Profiles;
using CalculadoraCedears.Api.Unit.Tests.Base;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Cedears.Profiles
{
    public class CedearsQueryHandlerProfileTests : AutoMapperBaseTests<CedearsProfile>
    {
        [Fact]
        public void Should_be_correctly_configured()
        {
            // Assert
            Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}