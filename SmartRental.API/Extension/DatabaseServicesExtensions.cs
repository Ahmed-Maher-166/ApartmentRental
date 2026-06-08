using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartRental.Core.Models.Identity;
using SmartRental.Reporisitory.Data;

namespace SmartRental.Api.Extension
{
    public static class DatabaseServicesExtensions
    {
        public static IServiceCollection AddDatabaseServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<SmartRentalContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<SmartRentalContext>()
                .AddDefaultTokenProviders();

            return services;
        }
      
        public static async Task ApplyMigrationsAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbContext = services.GetRequiredService<SmartRentalContext>();

                await dbContext.Database.MigrateAsync();

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await SmartRentalContextSeed.SeedRolesAsync(roleManager);

                var userManager = services.GetRequiredService<UserManager<AppUser>>();

                await SmartRentalContextSeed.GetDataSeedAsync(dbContext, userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger("Program");
                logger.LogError(ex, "An error occurred during applying migrations or seeding data.");
            }
        }
    }
}