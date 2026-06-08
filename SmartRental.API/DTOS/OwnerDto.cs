using System.ComponentModel.DataAnnotations;

namespace SmartRental.API.DTOS
{
    public class OwnerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> PhoneNumber { get; set; }

        public string Photo { get; set; }
       
        public string NationalID { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
