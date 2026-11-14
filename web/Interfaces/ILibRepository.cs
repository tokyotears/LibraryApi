using web.Models;
using web.DTO;

namespace web.Interfaces;

public interface ILibRepository {
	Task<List<Library>> GetLibraries();
	Task<List<Library>> GetMyLibraries(Guid id);
	Task<Library?> GetLibrary(Guid userId, Guid id);
	Task AddLibrary(Library library);
	Task<bool> DeleteLibrary(Guid id);
	Task<List<Book>> GetBooks(Guid id);
	Task AddBook(Book book);
	Task<bool> DeleteBook(Guid bookId);
	Task<bool> UpdateLibrary(LibAddRequest req, Guid id);
}
