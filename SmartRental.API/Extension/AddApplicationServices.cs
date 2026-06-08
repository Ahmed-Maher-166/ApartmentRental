using SmartRental.Api.Helper;

using Microsoft.AspNetCore.Mvc;
using SmartRental.Core.Repository;
using SmartRental.Core.Services;
using ApartmentRental.Service;
using SmartRental.Reporisitory;

namespace SmartRental.Api.Extension
{
    public static class AddApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(typeof(MappingProfiles));
            return services;
        }
    }
}
