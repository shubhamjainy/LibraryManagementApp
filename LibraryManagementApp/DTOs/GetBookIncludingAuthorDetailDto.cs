using LibraryManagementApp.Entities;

namespace LibraryManagementApp.DTOs
{
    public class GetBookIncludingAuthorDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public AuthorDto Author { get; set; }
    }
}
