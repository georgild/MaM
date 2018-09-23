using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.AspNetDummyParents;
using Models.Identity;
using Models.VFileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Contexts {
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options) {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<VFileSystemItem> VFileSystemItems { get; set; }
    }
}
