using web.Models;

namespace web.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetBook(Guid bookId);
    Task<List<Book>> GetBooks(Guid bookId);
    Task AddBook(Book book);
    Task UpdateBook(Book book);
    Task DeleteBook(Guid bookId); 
}