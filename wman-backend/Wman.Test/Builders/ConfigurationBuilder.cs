using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Wman.Test.Builders
{
    public class AspConfigurationBuilder
    {
        public static IConfiguration GetConfiguration()
        {
            var cfg = new Dictionary<string, string>
            {
                { "SigningKey", "TestValueAJKSJDJ2732636auhsdnh"}
            };

            IConfiguration cfgBuild = new ConfigurationBuilder().AddInMemoryCollection(cfg).Build();

            return cfgBuild;
        }

        public static IConfiguration GetConfigurationWithCloudinary()
        {
            var cfg = new Dictionary<string, string>
            {
                { "SigningKey", "TestValueAJKSJDJ2732636auhsdnh"}
            };

            IConfiguration cfgBuild = new ConfigurationBuilder().AddInMemoryCollection(cfg).Build();

            return cfgBuild;
        }
    }
}
