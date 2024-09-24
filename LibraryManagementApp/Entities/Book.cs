namespace LibraryManagementApp.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Desciption { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public ICollection<BookAllocation> BookAllocations { get; set; }
    }
}
