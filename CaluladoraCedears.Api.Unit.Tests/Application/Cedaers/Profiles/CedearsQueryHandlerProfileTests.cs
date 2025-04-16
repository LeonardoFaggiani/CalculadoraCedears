using CaluladoraCedears.Api.Application.Samples.Profiles;
using CaluladoraCedears.Api.Unit.Tests.Base;

namespace CaluladoraCedears.Api.Unit.Tests.Application.Samples.Profiles
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