using web.DTO;
using web.Enums;
using web.Interfaces;
using web.Models;

namespace web.Services;

public class BookService(IBookRepository bookRepository, IRequestValidationDispatcher validationDispatcher)
{
    public async Task<Response<IEnumerable<Book>>> GetBooks(Guid userId, Guid libId)
    {
        var res = await bookRepository.GetBooks(userId, libId);

        return res.Count == 0
            ? new Response<IEnumerable<Book>> { Error = ErrorType.BookNotFound }
            : new Response<IEnumerable<Book>> { Data = res };
    }

    public async Task<Response<Book>> GetBook(Guid userId, Guid libId, Guid bookId)
    {
        var res = await bookRepository.GetBook(userId, libId, bookId);

        return res is null ? new Response<Book> { Error = ErrorType.BookNotFound } : new Response<Book> { Data = res };
    }
    
    public async Task<Response> AddBook(Guid userId, Guid libId, BookCreateRequest req)
    {
        var validationRes = await validationDispatcher.ValidateAsync(req);

        if (!validationRes.IsValid)
            return new Response
                { Error = ErrorType.InvalidInput, Errors = [.. validationRes.Errors.Select(e => e.ErrorMessage)] };

        var res = await bookRepository.AddBook(userId, libId, req);
        
        return res == RepoStatus.Success ? new Response() : new Response { Error = ErrorType.LibNotFound }; 
    }
    
    public async Task<Response> DeleteBook(Guid userId, Guid libId, Guid bookId)
    {
        var res = await bookRepository.DeleteBook(userId, libId, bookId);
        
        return res == RepoStatus.Success ? new Response() : new Response { Error = ErrorType.BookNotFound }; 
    }

    public async Task<Response> UpdateBook(Guid userId, Guid libId, Guid bookId, BookUpdateRequest req)
    {
        var validationRes = await validationDispatcher.ValidateAsync(req);

        if (!validationRes.IsValid)
            return new Response
                { Error = ErrorType.InvalidInput, Errors = [.. validationRes.Errors.Select(e => e.ErrorMessage)] };

        var res = await bookRepository.UpdateBook(userId, libId, bookId, req);

        return res == RepoStatus.Success ? new Response() : new Response { Error = ErrorType.BookNotFound };
    }
}