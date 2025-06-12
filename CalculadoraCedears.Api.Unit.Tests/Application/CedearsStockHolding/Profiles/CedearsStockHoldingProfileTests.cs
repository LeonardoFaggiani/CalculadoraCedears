using CalculadoraCedears.Api.Application.CedearsStockHolding.Profiles;
using CalculadoraCedears.Api.Unit.Tests.Base;

namespace CalculadoraCedears.Api.Unit.Tests.Application.CedearsStockHolding.Profiles
{
    public class CedearsStockHoldingProfileTests : AutoMapperBaseTests<CedearsStockHoldingProfile>
    {
        [Fact]
        public void Should_be_correctly_configured()
        {
            // Assert
            Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
