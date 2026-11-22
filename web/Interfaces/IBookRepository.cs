using web.DTO;
using web.Enums;
using web.Models;

namespace web.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetBook(Guid userId, Guid libId, Guid bookId);
    Task<List<Book>> GetBooks(Guid userId, Guid libId);
    Task<RepoStatus> AddBook(Guid userId, Guid libId, BookCreateRequest req);
    Task<RepoStatus> UpdateBook(Guid userId, Guid libId, Guid bookId, BookUpdateRequest req);
    Task<RepoStatus> DeleteBook(Guid userId, Guid libId, Guid bookId); 
}