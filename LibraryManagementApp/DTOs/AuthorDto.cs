using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.DTOs
{
    public class AuthorDto
    {
         public int Id { get; set; }

        [Required(ErrorMessage = "Author Name is required")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        [MinLength(3, ErrorMessage = "Name cannot below 3 characters")]
        public string Name { get; set; }

        [MaxLength(150, ErrorMessage = "Description cannot exceed 150 characters")]
        public string Description { get; set; }
    }
}
