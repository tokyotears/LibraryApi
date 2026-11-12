using web.Interfaces;
using web.Models;
using web.Validation;
using web.DTO;
using web.Enums;

namespace web.Services;

public class LibService(ILibRepository libRepository, LibAddReqValidator libValidator, BookAddReqValidator bookValidator) : ILibService {
	public async Task<Response<Library>> GetLibrary(Guid id) {
		var library = await libRepository.GetLibrary(id);
		return library is null ? new Response<Library> { Error = ErrorType.LibNotFound } : new Response<Library> { Data = library };
	}

	public async Task<Response<IEnumerable<Library>>> GetAll() {
		var libraries = await libRepository.GetLibraries();
		return new Response<IEnumerable<Library>> { Data = libraries };
	}

	public async Task<Response<IEnumerable<Book>>> GetBooks(Guid id) {
		var library = await libRepository.GetLibrary(id);
		if (library is null) return new Response<IEnumerable<Book>> { Error = ErrorType.LibNotFound };
		
		var books = await libRepository.GetBooks(id);
		return new Response<IEnumerable<Book>> { Data = books };
	}

	public async Task<Response> AddLibrary(Guid id, LibAddRequest lib) {
		if (!(await libValidator.ValidateAsync(lib)).IsValid) return new Response {Error = ErrorType.InvalidInput};
		var library = await libRepository.GetLibrary(id);
		if (library is not null) return new Response { Error = ErrorType.LibExists };
		library = new Library(Guid.NewGuid(), id, lib.Name, lib.Desc, DateTime.UtcNow);
		await libRepository.AddLibrary(library);
		return new Response();
	}

	public async Task<Response> AddBook(Guid id, BookAddRequest book) {
		if (!(await bookValidator.ValidateAsync(book)).IsValid) return new Response { Error = ErrorType.InvalidInput };
		var library = await libRepository.GetLibrary(id);
		if (library is null) return new Response { Error = ErrorType.LibNotFound };
		await libRepository.AddBook(library.Id, book.Title, book.Author);
		return new Response();
	}

	public async Task<Response> DeleteBook(Guid id, Guid bookId) {
		var library = await libRepository.GetLibrary(id);
		if (library is null) return new Response { Error = ErrorType.LibNotFound };
		await libRepository.DeleteBook(id, bookId);
		return new Response();
	}

	public async Task<Response> UpdateLibrary(Guid id, LibAddRequest req) {
		if (!await libRepository.UpdateLibrary(req, id)) return new Response {Error = ErrorType.LibNotFound};
		return new Response();
	}

	public async Task<Response> DeleteLibrary(Guid id) {
		if (!await libRepository.DeleteLibrary(id)) return new Response {Error = ErrorType.LibNotFound};
		return new Response();
	}
}
