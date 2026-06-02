using ApartmentRental.Core.Models;

namespace AparatmentRental.Api.DTOS
{
    public class ApartmentDTO
    {
        
        public byte Rooms { get; set; }
        public byte Bathrooms { get; set; }
        public byte MaxTenants { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public Status AvailabilityStatus { get; set; } = Status.Available;
        public string City { get; set; }
        public string StreetName { get; set; }
        public int BuildingNumber { get; set; }
        public byte ApartmentNumber { get; set; }
        public byte FloorNumber { get; set; }
        public int OwnerId { get; set; }
        public List<string> Photos { get; set; } = new List<string>();
    }
}
