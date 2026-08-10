using web.Models;
using web.DTO;
using web.Validation;

namespace web.Interfaces;

public interface ILibService {
	Task<Response<IEnumerable<Library>>> GetAll(ILibRepository libRepository);
	Task<Response<Library>> GetLibrary(ILibRepository libRepository, Guid id);
	Task<Response<IEnumerable<Book>>> GetBooks(ILibRepository libRepository, Guid id);
	Task<Response> AddLibrary(ILibRepository libRepository, LibAddReqValidator validator, Guid id, LibAddRequest lib);
	Task<Response> AddBook(ILibRepository libRepository, BookAddReqValidator validator, Guid id, BookAddRequest book);
	Task<Response> DeleteBook(ILibRepository libRepository, Guid id, Guid bookId);
	Task<Response> UpdateLibrary(ILibRepository libRepository, Guid id, LibAddRequest req);
	Task<Response> DeleteLibrary(ILibRepository libRepository, Guid id);
}
