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
    public class RentalContractConfigration : IEntityTypeConfiguration<RentalContract>
    {
        public void Configure(EntityTypeBuilder<RentalContract> builder)
        {
           builder.HasOne(rc => rc.Apartment)
                    .WithMany(a => a.RentalContracts)
                    .HasForeignKey(rc => rc.ApartmentId)
                    .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(rc => rc.Tenant)
                    .WithMany(t => t.RentalContracts)
                    .HasForeignKey(rc => rc.TenantId)
                    .OnDelete(DeleteBehavior.NoAction);
             builder.Property(rc => rc.StartDate).IsRequired();
             builder.Property(rc => rc.EndDate).IsRequired();
             builder.Property(rc => rc.AnnualIncreasePercentage).IsRequired();
        }
    }
}
