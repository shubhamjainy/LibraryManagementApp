using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.DTOs
{
    public class UserDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(50, ErrorMessage = "Email cannot exceed 50 characters")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [MaxLength(10, ErrorMessage = "Phone number cannot exceed 10 characters")]
        [MinLength(10, ErrorMessage = "Phone number cannot below 10 characters")]
        public string PhoneNo { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters")]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Aadhar number must be exactly 16 digits and numeric")]
        public string AadharNo { get; set; }
    }
}
