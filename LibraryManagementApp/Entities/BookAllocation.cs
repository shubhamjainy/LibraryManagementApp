
namespace LibraryManagementApp.Entities
{
    public class BookAllocation
    {
        public int Id { get; set; }
        public DateTime AllocationDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }
    }
}
