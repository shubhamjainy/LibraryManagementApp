using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.DTOs
{
    public class BookAllocationDto
    {
        [Required]
        public DateOnly AllocationDate { get; set; }

        [Required]
        public DateOnly ReturnDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }
    }
}
