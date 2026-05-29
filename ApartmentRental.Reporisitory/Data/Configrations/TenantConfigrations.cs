using ApartmentRental.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Reporisitory.Data.Configrations
{
    public class TenantConfigrations : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasOne(t => t.University)
                   .WithMany(u => u.Tenants)
                   .HasForeignKey(t => t.UniversityId)
                   .OnDelete(DeleteBehavior.SetNull);
            builder.HasMany(t => t.RentalContracts)
                   .WithOne(rc => rc.Tenant)
                   .HasForeignKey(rc => rc.TenantId)
                   .OnDelete(DeleteBehavior.Cascade);
          
         
        }
    }
}
