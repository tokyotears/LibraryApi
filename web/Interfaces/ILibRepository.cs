using web.Models;

namespace web.Interfaces;

public interface ILibRepository {
	Task<List<Library>> GetLibraries();
	Task<Library?> GetLibrary(Guid id);
	Task AddLibrary(Library library);
	Task<bool> DeleteLibrary(Guid id);
	Task<List<Book>> GetBooks(Guid id);
	Task AddBook(Guid id, string author, string title);
	Task<bool> DeleteBook(Guid id, Guid bookId);
}
