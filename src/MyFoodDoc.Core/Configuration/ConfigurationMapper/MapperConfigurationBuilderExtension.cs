using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace MyFoodDoc.Core.Configuration.ConfigurationMapper
{
    public static class MapperConfigurationBuilderExtension
    {
        public static IConfigurationBuilder WithJsonMapping(this IConfigurationBuilder builder, string path)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            var unmappedConfig = builder.Build();

            var mappingBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path, optional: true, reloadOnChange: true);

            builder.Sources.Clear();
            builder.AddConfiguration(unmappedConfig);
            builder.Add(new MapperConfigurationSource { Mapping = mappingBuilder.Build(), Configuration = unmappedConfig });

            return builder;
        }
    }
}
