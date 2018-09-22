using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.AspNetDummyParents;
using Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Contexts {
    public class Context // : DbContext
        : IdentityDbContext
    <User,
        Role,
        Guid,
        //These should be ignored. Literally.
        CustomIdentityUserClaim,
        CustomIdentityUserRole,
        CustomIdentityUserLogin,
        CustomIdentityRoleClaim,
        CustomIdentityUserToken> 
    {
        public Context(DbContextOptions<Context> options)
            : base(options) {
        }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
