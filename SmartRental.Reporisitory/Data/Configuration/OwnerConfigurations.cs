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
    public class OwnerConfigurations : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.HasOne(o => o.AppUser)
                   .WithOne(u => u.Owner)
                   .HasForeignKey<Owner>(o => o.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(o => o.AppUserId)
                   .IsUnique();
        }
    }
}
