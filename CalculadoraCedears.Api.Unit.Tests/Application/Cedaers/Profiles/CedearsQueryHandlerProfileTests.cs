using CalculadoraCedears.Api.Application.Cedears.Profiles;
using CalculadoraCedears.Api.Unit.Tests.Base;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Cedaers.Profiles
{
    public class CedearsQueryHandlerProfileTests : AutoMapperBaseTests<CedearsQueryHandlerProfile>
    {
        [Fact]
        public void Should_be_correctly_configured()
        {
            // Assert
            Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}