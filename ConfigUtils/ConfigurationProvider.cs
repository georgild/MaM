using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigUtils {
    public class ConfigurationProvider {

        private static IConfiguration configuration = null;

        static ConfigurationProvider() {

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            configuration = builder.Build();
        }

        public static IConfiguration GetDefaultConfig() {

            return configuration;
        }
    }
}
