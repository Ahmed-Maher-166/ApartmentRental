using System.ComponentModel.DataAnnotations;

namespace SmartRental.API.DTOS
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public List<string> PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Photo { get; set; }
        [Required]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "National ID must be exactly 14 characters long.")]
        public string NationalID { get; set; }
        [Required]
        public string Role { get; set; }
        public int? UniversityId { get; set; }

    }


}
