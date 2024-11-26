using System.ComponentModel.DataAnnotations;

namespace SensorApis.Models
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
    