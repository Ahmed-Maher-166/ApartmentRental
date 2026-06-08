using SmartRental.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Reporisitory.Data.Configuration
{
    public class ApartmentConfigrations : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            builder.HasOne(a => a.Owner)
                   .WithMany(o => o.Apartments)
                   .HasForeignKey(a => a.OwnerId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(a => a.Photos)
                   .WithOne(p => p.Apartment)
                   .HasForeignKey(p => p.ApartmentId)
                   .OnDelete(DeleteBehavior.Cascade);
             builder.HasMany(a => a.RentalContracts)
                   .WithOne(rc => rc.Apartment)
                   .HasForeignKey(rc => rc.ApartmentId)
                   .OnDelete(DeleteBehavior.Cascade);
                    builder.Property(a => a.City).HasColumnType("varchar(35)").IsRequired();
                    builder.Property(a => a.StreetName).HasColumnType("varchar(35)").IsRequired();
                    builder.Property(a => a.BuildingNumber).IsRequired();
                    builder.Property(a => a.ApartmentNumber).IsRequired();
                    builder.Property(a => a.FloorNumber).IsRequired();
                    builder.Property(a => a.Price).IsRequired();
                    builder.Property(a => a.Rooms).IsRequired();
                    builder.Property(a => a.Bathrooms).IsRequired();
                    builder.Property(a => a.MaxTenants).IsRequired();
            
        }
    }
}
