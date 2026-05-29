
using AparatmentRental.Api.Extension;
using ApartmentRental.Core.Models.Identity;
using ApartmentRental.Reporisitory.Data;
using ApartmentRental.Reporisitory.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AparatmentRental.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            #region MAIGGration Database
            builder.Services.AddDbContext<ApartmentRentalContext>(options =>
                                     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                             );
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                                   options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"))
                           );
            #endregion
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(builder.Configuration["ProjectFrontend"])
                        ;
                });
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddApplicationServices();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:ValidAudience"],

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])
                        )
                    };
                });
            // Configure the HTTP request pipeline.

            #region Update-Database
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var _dbContext = services.GetRequiredService<StoredContext>();
                await _dbContext.Database.MigrateAsync();
                var _dbContextIdentity = services.GetRequiredService<AppIdentityDbContext>();
                await _dbContextIdentity.Database.MigrateAsync();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(userManager);
                await StoredContextSeed.GetDataSeedkAsync(_dbContext);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>(); // Logger for exception handling
                logger.LogError(ex, "An error occurred during applying the migration.");
            }

            #endregion
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
