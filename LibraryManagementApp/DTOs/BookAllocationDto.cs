using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.DTOs
{
    public class BookAllocationDto
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }
    }

}
