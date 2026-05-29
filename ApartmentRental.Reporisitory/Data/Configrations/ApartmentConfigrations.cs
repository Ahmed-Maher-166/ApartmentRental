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
    public class ApartmentConfigrations : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.HasOne(a => a.Owner)
                   .WithMany(o => o.Apartments)
                   .HasForeignKey(a => a.OwnerId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Property(a => a.City)
                    .IsRequired()
                    .HasColumnType("varchar(20)");
            builder.Property(a => a.StreetName)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            builder.HasMany(a => a.Photos)
                   .WithOne(p => p.Apartment)
                   .HasForeignKey(p => p.ApartmentId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(a => a.RentalContracts)
                   .WithOne(rc => rc.Apartment)
                   .HasForeignKey(rc => rc.ApartmentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
