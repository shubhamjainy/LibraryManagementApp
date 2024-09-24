namespace LibraryManagementApp.Interfaces
{
    public interface IBookService
    {
        void AddBook();
        void RemoveBook();
        void UpdateBook();
        void GetAllBooks();
        void GetBookByID();
        void GetBookByName();
        void GetBookByGener();
        void GetBookByAuthorID();
    }
}
