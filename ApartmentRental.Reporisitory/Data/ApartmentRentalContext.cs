using ApartmentRental.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Reporisitory.Data
{
    public class ApartmentRentalContext : DbContext
    {
        public ApartmentRentalContext(DbContextOptions<ApartmentRentalContext> options) : base(options)
        {
        }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Photos> Photos { get; set; }
        public DbSet<Phones> Phones { get; set; }
        public DbSet<RentalContract> RentalContracts { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<University> Universities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
