using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApp.DTOs
{
    public class GetAuthorIncludingBooksDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<BookDto> Books { get; set; }
    }
}
