using System.ComponentModel.DataAnnotations;

namespace SmartRental.API.DTOS
{
    public class UniversityDTO
    {
        [Required]
        public string Name { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
    }
}
