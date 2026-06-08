using SmartRental.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartRental.API.DTOS
{
    public class ApartmentDTO
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        [Range(1, byte.MaxValue)]

        public byte Rooms { get; set; }
        [Range(1, byte.MaxValue)]
        public byte Bathrooms { get; set; }
        [Range(1, byte.MaxValue)]
        public byte MaxTenants { get; set; }
        public string Description { get; set; }
        [Range(1, int.MaxValue)]

        public int Price { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public Gender Gender { get; set; } = Gender.Male;
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public Status AvailabilityStatus { get; set; } = Status.Available;
        public string City { get; set; }
        public string StreetName { get; set; }
        public int BuildingNumber { get; set; }
        [Range(1, byte.MaxValue)]

        public byte ApartmentNumber { get; set; }
        [Range(1, byte.MaxValue)]

        public byte FloorNumber { get; set; }
        public int OwnerId { get; set; }
        public List<PhotoDTO> Photos { get; set; } = new List<PhotoDTO>();
    }
}
