using Infrastructure.Security.Idetity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Security.Identity
{
    public class SecurityContext:IdentityDbContext<ApplicationUser,ApplicationRole,int>
    {
        public SecurityContext(DbContextOptions<SecurityContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            builder.Entity<ApplicationUser>().Property(x => x.LastName).IsRequired(false).HasMaxLength(100);
            builder.Entity<ApplicationUser>().Property(x => x.ProfileImageUrl).IsRequired(false).HasMaxLength(500);
            builder.Entity<ApplicationUser>().Property(x => x.Bio).IsRequired(false).HasMaxLength(5000);
            builder.Entity<ApplicationUser>().Property(x => x.RefreshToken).IsRequired(false).HasMaxLength(500);
            builder.Entity<ApplicationUser>().Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);
            






            base.OnModelCreating(builder);
        }

    }
}
