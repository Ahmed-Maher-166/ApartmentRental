using ApartmentRental.Core.Repository;
using ApartmentRental.Reporisitory;
using Microsoft.AspNetCore.Mvc;

namespace AparatmentRental.Api.Extension
{
    public static class AddApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
