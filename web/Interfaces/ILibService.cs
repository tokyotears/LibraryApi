using web.Models;
using web.DTO;

namespace web.Interfaces;

public interface ILibService {
	Task<Response<Library>> GetLibrary(Guid id);
	Task<Response<IEnumerable<Library>>> GetAll();
	Task<Response<IEnumerable<Book>>> GetBooks(Guid id);
	Task<Response> AddLibrary(Guid id, LibAddRequest lib);
	Task<Response> AddBook(Guid id, BookAddRequest book);
	Task<Response> DeleteBook(Guid id, Guid bookId);
	Task<Response> UpdateLibrary(Guid id, LibAddRequest req);
	Task<Response> DeleteLibrary(Guid id);
}
