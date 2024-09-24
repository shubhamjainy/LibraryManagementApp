using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.DTOs
{
    public class UserUpdateDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        [Phone]
        [MaxLength(10, ErrorMessage = "Phone number cannot exceed 10 characters")]
        [MinLength(10, ErrorMessage = "Phone number cannot below 10 characters")]
        public string PhoneNo { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters")]
        public string Address { get; set; }

    }
}
