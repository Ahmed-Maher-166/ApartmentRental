using AparatmentRental.Api.Helper;
using ApartmentRental.Core.Repository;
using ApartmentRental.Core.Services;
using ApartmentRental.Reporisitory;
using ApartmentRental.Service;
using Microsoft.AspNetCore.Mvc;

namespace AparatmentRental.Api.Extension
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
