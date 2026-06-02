using AparatmentRental.Api.DTOS;
using ApartmentRental.Core.Models;
using AutoMapper;

namespace AparatmentRental.Api.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {
            CreateMap<Apartment, ApartmentDTO>()
                .ForMember(dest => dest.Photos, opt => opt.MapFrom<ImageApartmentUrlResolver>()).ReverseMap();


        }
    }
}
