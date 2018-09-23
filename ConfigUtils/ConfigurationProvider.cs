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
                //.SetBasePath(@"D:\University\Дипломна работа\src\project\MaM\WebApi")
                .AddJsonFile(@"D:\University\Дипломна работа\src\project\MaM\WebApi\appsettings.json");

            configuration = builder.Build();
        }

        public static IConfiguration GetDefaultConfig() {

            return configuration;
        }
    }
}
