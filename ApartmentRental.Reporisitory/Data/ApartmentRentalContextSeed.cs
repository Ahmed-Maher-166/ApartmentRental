using ApartmentRental.Core.Models;
using ApartmentRental.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Reporisitory.Data
{
    public static class ApartmentRentalContextSeed
    {
        public static async Task GetDataSeedkAsync(ApartmentRentalContext dbContext)
        {
            var PathIntial = Path.Combine("../ApartmentRental.Repository", "Data", "DataSeed");
            await SeedDataFromJsonAsync<Apartment>(dbContext, Path.Combine(PathIntial, "apartments.json"));
            await SeedDataFromJsonAsync<Owner>(dbContext, Path.Combine(PathIntial, "owners.json"));
            await SeedDataFromJsonAsync<Photos>(dbContext, Path.Combine(PathIntial, "photos.json"));
            await SeedDataFromJsonAsync<Phones>(dbContext, Path.Combine(PathIntial, "phones.json"));
            await SeedDataFromJsonAsync<RentalContract>(dbContext, Path.Combine(PathIntial, "rentalContracts.json"));
            await SeedDataFromJsonAsync<Tenant>(dbContext, Path.Combine(PathIntial, "tenants.json"));
            await SeedDataFromJsonAsync<University>(dbContext, Path.Combine(PathIntial, "universities.json"));
            await SeedDataFromJsonAsync<AppUser>(dbContext, Path.Combine(PathIntial, "appUsers.json"));
        }
        public static async Task SeedDataFromJsonAsync<T>(ApartmentRentalContext context, string jsonFilePath) where T : class
        {
            var data = File.ReadAllText(jsonFilePath);
            var items = System.Text.Json.JsonSerializer.Deserialize<List<T>>(data);
            if (items?.Count <= 0)
            {
                foreach (var item in items)
                  await context.Set<T>().AddAsync(item);                
                await context.SaveChangesAsync();
            }
        }
    }
}
