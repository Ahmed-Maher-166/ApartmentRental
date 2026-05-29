using ApartmentRental.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Reporisitory.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Owner", "Tenant" };
            foreach (var role in roles)
              if (!await roleManager.RoleExistsAsync(role))
                 await roleManager.CreateAsync(new IdentityRole(role));
         }
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    Email = "Ahmedre126@gmail.com",
                    UserName = "Ahmed.BackEnd",
                    PhoneNumber = "01158238898",

                    Photo = "default-user.png",
                    NationalID = "00000000000000",
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, "Password123!");

                if (result.Succeeded)
                 await userManager.AddToRoleAsync(user, "Owner");                
            }
        }
    }
}
