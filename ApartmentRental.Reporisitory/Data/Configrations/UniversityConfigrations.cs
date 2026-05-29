using ApartmentRental.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Reporisitory.Data.Configrations
{
    public class UniversityConfigrations : IEntityTypeConfiguration<University>
    {
        public void Configure(EntityTypeBuilder<University> builder)
        {
            builder.HasMany(u => u.Tenants)
                   .WithOne(t => t.University)
                   .HasForeignKey(t => t.UniversityId)
                   .OnDelete(DeleteBehavior.SetNull);
            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasColumnType("varchar(20)");
            builder.Property(u => u.City)
                     .IsRequired()
                     .HasColumnType("varchar(20)");
           builder.Property(u => u.Area)
                     .IsRequired()
                     .HasColumnType("varchar(30)");
        }
    }
}
