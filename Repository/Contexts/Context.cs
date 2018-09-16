using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.AspNetDummyParents;
using Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Contexts {
    public class Context : IdentityDbContext
    <User,
        Role,
        Guid,
        //These should be ignored. Literally.
        CustomIdentityUserClaim,
        CustomIdentityUserRole,
        CustomIdentityUserLogin,
        CustomIdentityRoleClaim,
        CustomIdentityUserToken> {
        public Context(DbContextOptions<Context> options)
            : base(options) {
        }

        /// <summary>
        /// The DbSet for the RefreshTokens table.
        /// </summary>
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
