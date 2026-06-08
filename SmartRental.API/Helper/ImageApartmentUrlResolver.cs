using SmartRental.API.DTOS;
using SmartRental.Core.Models;
using AutoMapper;

namespace SmartRental.API.Helper
{
    public class ImageApartmentUrlResolver : IValueResolver<Apartment, ApartmentDTO, List<PhotoDTO>>
    {
        private readonly IConfiguration _config;

        public ImageApartmentUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        public List<PhotoDTO> Resolve(
          Apartment source,
          ApartmentDTO destination,
          List<PhotoDTO> destMember,
          ResolutionContext context)
        {
            if (source.Photos == null || !source.Photos.Any())
                return new List<PhotoDTO>();
            var baseUrl = _config["ProjectFrontend"];

            return source.Photos
                .Where(photo => !string.IsNullOrWhiteSpace(photo.PhotoUrl))
                .Select(photo => new PhotoDTO
                {
                    Id = photo.Id,
                    PhotoUrl = photo.PhotoUrl.StartsWith("http")
                        ? photo.PhotoUrl
                        : $"{baseUrl}/{photo.PhotoUrl}"
                })
                .ToList();
        }
    }
}
