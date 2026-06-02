using ApartmentRental.Core.Models.Identity;
using ApartmentRental.Reporisitory.Data;
using ApartmentRental.Reporisitory.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AparatmentRental.Api.Extension
{
    public static class DatabaseServicesExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApartmentRentalContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

            return services;
        }

        public static async Task ApplyMigrationsAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbContext = services.GetRequiredService<ApartmentRentalContext>();
                await dbContext.Database.MigrateAsync();

                var dbContextIdentity = services.GetRequiredService<AppIdentityDbContext>();
                await dbContextIdentity.Database.MigrateAsync();

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await AppIdentityDbContextSeed.SeedRolesAsync(roleManager);

                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(userManager);
                await ApartmentRentalContextSeed.GetDataSeedkAsync(dbContext);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger("Program");
                logger.LogError(ex, "An error occurred during applying the migration.");
            }
        }
    }
}
