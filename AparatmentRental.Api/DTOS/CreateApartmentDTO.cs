using ApartmentRental.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace AparatmentRental.Api.DTOS
{
    public class CreateApartmentDTO
    {

        [Range(1, byte.MaxValue)]

        public byte Rooms { get; set; }
        [Range(1, byte.MaxValue)]
        public byte Bathrooms { get; set; }
        [Range(1, byte.MaxValue)]
        public byte MaxTenants { get; set; }
        public string Description { get; set; }
        [Range(1, int.MaxValue)]

        public int Price { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public Status AvailabilityStatus { get; set; } = Status.Available;

        public string City { get; set; }

        public string StreetName { get; set; }
        public int BuildingNumber { get; set; }
        [Range(1, byte.MaxValue)]

        public byte ApartmentNumber { get; set; }
        [Range(1, byte.MaxValue)]

        public byte FloorNumber { get; set; }
        public List<string> Photos { get; set; } = new List<string>();
    }
}
