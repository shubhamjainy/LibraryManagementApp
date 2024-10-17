using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.DTOs
{
    public class BookAllocationDto
    {
        public int Id { get; set; }

        public DateTime AllocationDate { get; set; }

        public DateTime ReturnDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }
    }

}
