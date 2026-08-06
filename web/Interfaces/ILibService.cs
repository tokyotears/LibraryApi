using web.Models;
using web.DTO;

namespace web.Interfaces;

public interface ILibService {
	Task<Response<IEnumerable<Library>>> GetAll(ILibRepository libRepository);
	Task<Response<Library>> GetLibrary(ILibRepository libRepository, Guid id);
	Task<Response<IEnumerable<Book>>> GetBooks(ILibRepository libRepository, Guid id);
	Task<Response> AddLibrary(ILibRepository libRepository, Guid id, LibAddRequest lib);
	Task<Response> AddBook(ILibRepository libRepository, Guid id, BookAddRequest book);
	Task<Response> DeleteBook(ILibRepository libRepository, Guid id, Guid bookId);
}
