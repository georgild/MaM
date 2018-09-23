using ConfigUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Repository.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.UpgradeFactory {
    public class AppContextFactory : IDesignTimeDbContextFactory<Context> {
        public Context CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.
                UseMySql(
                     ConfigUtils.ConfigurationProvider
                    .GetDefaultConfig()
                    .GetSection("ConnectionStrings").GetValue<string>("MainDB")
                );

            return new Context(optionsBuilder.Options);
        }
    }
}
