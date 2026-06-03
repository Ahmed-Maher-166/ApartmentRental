using AparatmentRental.Api.DTOS;
using ApartmentRental.Core.Models;
using AutoMapper;

namespace AparatmentRental.Api.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {
            CreateMap<Photos, PhotoDTO>();
            CreateMap<Apartment, ApartmentDTO>()
               .ForMember(dest => dest.Photos,
               opt => opt.MapFrom<ImageApartmentUrlResolver>());
            CreateMap<CreateApartmentDTO, Apartment>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.OwnerId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Owner,
                    opt => opt.Ignore())
                .ForMember(dest => dest.RentalContracts,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Photos,
                    opt => opt.MapFrom(src =>
                        src.Photos.Select(url => new Photos
                        {
                            PhotoUrl = url
                        }).ToList()
                    ));

            CreateMap<UpdateApartmentDTO, Apartment>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.OwnerId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Owner,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Photos,
                    opt => opt.Ignore())
                .ForMember(dest => dest.RentalContracts,
                    opt => opt.Ignore());
        }
    }
}
