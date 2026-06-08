namespace SmartRental.API.DTOS
{
    public class UpdateOwnerDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public List<string>? PhoneNumber { get; set; }
        public string? Photo { get; set; }
    }
}
