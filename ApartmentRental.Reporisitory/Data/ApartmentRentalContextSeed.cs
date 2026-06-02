using ApartmentRental.Core.Models;
using ApartmentRental.Core.Models.Identity;
using Microsoft.EntityFrameworkCore;
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
            var PathIntial = Path.Combine("..", "ApartmentRental.Reporisitory", "Data", "DataSeed");
            await SeedDataFromJsonAsync<University>(dbContext, Path.Combine(PathIntial, "universities.json"));
            await SeedDataFromJsonAsync<Apartment>(dbContext, Path.Combine(PathIntial, "apartments.json"));

        }
        public static async Task SeedDataFromJsonAsync<T>(ApartmentRentalContext context, string jsonFilePath) where T : class
        {
            if (!await context.Set<T>().AnyAsync())
            {

                var data = File.ReadAllText(jsonFilePath);
                var items = System.Text.Json.JsonSerializer.Deserialize<List<T>>(data);
                if (items?.Count > 0)
                {
                    foreach (var item in items)
                        await context.Set<T>().AddAsync(item);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
