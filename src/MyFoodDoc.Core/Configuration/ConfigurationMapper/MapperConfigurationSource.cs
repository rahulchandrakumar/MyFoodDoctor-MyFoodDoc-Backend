using Microsoft.Extensions.Configuration;

namespace MyFoodDoc.Core.Configuration.ConfigurationMapper
{
    public class MapperConfigurationSource : IConfigurationSource
    {
        public IConfiguration Mapping { get; internal set; }

        public IConfiguration Configuration { get; internal set; }

        public virtual IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new MapperConfigurationProvider(this);
        }
    }
}
