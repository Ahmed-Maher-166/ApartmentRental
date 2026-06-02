using AparatmentRental.Api.DTOS;
using ApartmentRental.Core.Models;
using AutoMapper;

namespace AparatmentRental.Api.Helper
{
    public class ImageApartmentUrlResolver : IValueResolver<Apartment, ApartmentDTO, List<string>>
    {
        private readonly IConfiguration _config;

        public ImageApartmentUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        public List<string> Resolve(
          Apartment source,
          ApartmentDTO destination,
          List<string> destMember,
          ResolutionContext context)
        {
            if (source.Photos == null || !source.Photos.Any())
                return new List<string>();

            var baseUrl = _config["ProjectFrontend"];

            return source.Photos
                .Where(photo => !string.IsNullOrWhiteSpace(photo.PhotoUrl))
                .Select(photo =>
                    photo.PhotoUrl.StartsWith("http")
                        ? photo.PhotoUrl
                        : $"{baseUrl}/{photo.PhotoUrl}"
                )
                .ToList();
        }
    }
}
