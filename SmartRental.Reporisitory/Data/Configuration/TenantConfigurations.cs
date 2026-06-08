using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartRental.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Reporisitory.Data.Configuration
{
    public class TenantConfigurations : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasOne(t => t.AppUser)
                 .WithOne(u => u.Tenant)
                 .HasForeignKey<Tenant>(t => t.AppUserId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(t => t.AppUserId)
                   .IsUnique();

            builder.HasOne(t => t.University)
                   .WithMany(u => u.Tenants)
                   .HasForeignKey(t => t.UniversityId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.RentalContracts)
                   .WithOne(rc => rc.Tenant)
                   .HasForeignKey(rc => rc.TenantId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
