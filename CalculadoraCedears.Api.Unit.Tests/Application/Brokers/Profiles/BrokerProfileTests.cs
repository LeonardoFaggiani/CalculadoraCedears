using CalculadoraCedears.Api.Application.Brokers.Profiles;
using CalculadoraCedears.Api.Unit.Tests.Base;

namespace CalculadoraCedears.Api.Unit.Tests.Application.Brokers.Profiles
{
    public class BrokerProfileTests : AutoMapperBaseTests<BrokerProfile>
    {
        [Fact]
        public void Should_be_correctly_configured()
        {
            // Assert
            Mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
