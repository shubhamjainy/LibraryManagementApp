using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Book Name cannot exceed 150 characters")]
        [MinLength(2, ErrorMessage = "Book Name cannot below 2 characters")]
        public string Name { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Book Genre cannot exceed 150 characters")]
        public string Genre { get; set; }

        public string Desciption { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }
}
