using FluentValidation;
using web.DTO;
using web.Enums;
using web.Interfaces;
using web.Models;

namespace web.Services;

public class BookService(ILibRepository libRepository, IBookRepository bookRepository, IValidator<BookCreateRequest> bookAddValidator)
{
    public async Task<Response<IEnumerable<Book>>> GetBooks(Guid userId, Guid libId) 
    {
        var library = await libRepository.GetLibrary(userId, libId);
        if (library is null) return new Response<IEnumerable<Book>> { Error = ErrorType.LibNotFound };
		
        var books = await bookRepository.GetBooks(libId);
        return new Response<IEnumerable<Book>> { Data = books };
    }
    
    public async Task<Response> AddBook(Guid userId, Guid libId, BookCreateRequest req)
    {
        var validationRes = await bookAddValidator.ValidateAsync(req);

        if (!validationRes.IsValid)
            return new Response
                { Error = ErrorType.InvalidInput, Errors = [.. validationRes.Errors.Select(e => e.ErrorMessage)] };	
		
        var library = await libRepository.GetLibrary(userId, libId);
        if (library is null) return new Response { Error = ErrorType.LibNotFound };
        await bookRepository.AddBook(new Book(Guid.NewGuid(), req.Author, req.Title, library.Id));
        return new Response();
    }
    
    public async Task<Response> DeleteBook(Guid userId, Guid libId, Guid bookId) 
    {
        var library = await libRepository.GetLibrary(userId, libId);
        if (library is null) return new Response { Error = ErrorType.LibNotFound };
        await bookRepository.DeleteBook(bookId);
        return new Response();
    }

    public async Task<Response> UpdateBook(Guid userId, Guid libId, Guid bookId, BookUpdateRequest req)
    {
        var library = await libRepository.GetLibrary(userId, libId);
        if (library is null) return new Response { Error = ErrorType.Forbidden };

        var book = await bookRepository.GetBook(bookId);
        if (book is null) return new Response() { Error = ErrorType.BookNotFound };
        if (book.LibId != libId) return new Response { Error = ErrorType.Forbidden };

        await bookRepository.UpdateBook(book);
        
        return new Response();
    }
}