using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MyFoodDoc.Core.Configuration.ConfigurationMapper
{
    public class MapperConfigurationProvider : ConfigurationProvider
    {
        private readonly MapperConfigurationSource _source;

        public MapperConfigurationProvider(MapperConfigurationSource source)
        {
            _source = source;
        }

        public override void Load()
        {
            Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var mapping in _source.Mapping.AsEnumerable().Where(mapping => !string.IsNullOrEmpty(mapping.Value)))
            {
                var key = mapping.Key;
                var value = mapping.Value;
                try
                {
                    var valueWithReplacement = Regex.Replace(value, @"{(.*?)}", KeyLookup);
                    Data.Add(key, valueWithReplacement);
                }
                catch (ArgumentException)
                {
                }
            }
        }

        private string KeyLookup(Match m)
        {
            var lookupName = m.Groups[1].Value;
            var value = _source.Configuration?[lookupName];

            if (value == null)
            {
                throw new ArgumentException($"No value found for {lookupName}.");
            }
            return value;
        }
    }
}
