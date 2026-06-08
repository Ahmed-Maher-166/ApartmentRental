using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartRental.Core.Models;
using SmartRental.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Reporisitory.Data
{
    public static class SmartRentalContextSeed
    {
        public static async Task GetDataSeedAsync(
            SmartRentalContext dbContext,
            UserManager<AppUser> userManager)
        {
            var pathInitial = Path.Combine("..", "SmartRental.Reporisitory", "Data", "DataSeed");

            await SeedDataFromJsonAsync<University>(
                dbContext,
                Path.Combine(pathInitial, "universities.json"));

            await SeedUsersWithProfilesAsync(dbContext, userManager);

            await SeedDataFromJsonAsync<Apartment>(
                dbContext,
                Path.Combine(pathInitial, "apartments.json"));
        }
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Owner", "Tenant" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        private static async Task SeedUsersWithProfilesAsync(
            SmartRentalContext dbContext,
            UserManager<AppUser> userManager)
        {
            if (!await userManager.Users.AnyAsync())
            {
                var ownerUser = new AppUser
                {
                    Name = "Owner User",
                    UserName = "owner@test.com",
                    Email = "owner@test.com",
                    PhoneNumber = "01111111111",
                    Photo = "default-user.png",
                    NationalID = "11111111111111",
                    CreatedAt = DateTime.UtcNow
                };

                var ownerResult = await userManager.CreateAsync(ownerUser, "Password123!");

                if (ownerResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(ownerUser, "Owner");

                    await dbContext.Owners.AddAsync(new Owner
                    {
                        AppUserId = ownerUser.Id
                    });
                }

                var tenantUser = new AppUser
                {
                    Name = "TenantUser",
                    UserName = "tenant@test.com",
                    Email = "tenant@test.com",
                    PhoneNumber = "01222222222",
                    Photo = "default-user.png",
                    NationalID = "22222222222222",
                    CreatedAt = DateTime.UtcNow
                };

                var tenantResult = await userManager.CreateAsync(tenantUser, "Password123!");

                if (tenantResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(tenantUser, "Tenant");

                    await dbContext.Tenants.AddAsync(new Tenant
                    {
                        AppUserId = tenantUser.Id,
                        UniversityId = 1
                    });
                }

                await dbContext.SaveChangesAsync();
            }
        }

        public static async Task SeedDataFromJsonAsync<T>(
            SmartRentalContext context,
            string jsonFilePath) where T : class
        {
            if (!await context.Set<T>().AnyAsync())
            {
                var data = await File.ReadAllTextAsync(jsonFilePath);

                var items = System.Text.Json.JsonSerializer.Deserialize<List<T>>(
                    data,
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (items?.Count > 0)
                {
                    await context.Set<T>().AddRangeAsync(items);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
