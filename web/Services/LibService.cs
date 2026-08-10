using web.Interfaces;
using web.Models;
using web.Validation;
using web.DTO;

namespace web.Services;

public class LibService : ILibService {
	public async Task<Response<Library>> GetLibrary(ILibRepository libRepository, Guid id) {
		var library = await libRepository.GetLibrary(id);
		if (library is null) return new Response<Library> { Error = ErrorType.LibNotFound };
		return new Response<Library> { Data = library };
	}

	public async Task<Response<IEnumerable<Library>>> GetAll(ILibRepository libRepository) {
		var libraries = await libRepository.GetLibraries();
		return new Response<IEnumerable<Library>> { Data = libraries };
	}

	public async Task<Response<IEnumerable<Book>>> GetBooks(ILibRepository libRepository, Guid id) {
		var library = await libRepository.GetLibrary(id);
		if (library is null) return new Response<IEnumerable<Book>> { Error = ErrorType.LibNotFound };
		var books = await libRepository.GetBooks(id);
		return new Response<IEnumerable<Book>> { Data = books };
	}

	public async Task<Response> AddLibrary(ILibRepository libRepository, LibAddReqValidator validator, Guid id, LibAddRequest lib) {
		if (validator.Validate(lib).IsValid) {
			Library? library = await libRepository.GetLibrary(id);
			if (library is not null) return new Response { Error = ErrorType.LibExists };
			library = new(id, lib.Name, lib.Desc);
			await libRepository.AddLibrary(library);
			return new Response();
		} else return new Response {Error = ErrorType.InvalidInput};
	}

	public async Task<Response> AddBook(ILibRepository libRepository, BookAddReqValidator validator, Guid id, BookAddRequest book) {
		if (validator.Validate(book).IsValid) {
			Library? library = await libRepository.GetLibrary(id);
			if (library is null) return new Response { Error = ErrorType.LibNotFound };
			await libRepository.AddBook(library.Id, book.Title, book.Author);
			return new Response();
		} else return new Response { Error = ErrorType.InvalidInput };
	}

	public async Task<Response> DeleteBook(ILibRepository libRepository, Guid id, Guid bookId) {
		Library? library = await libRepository.GetLibrary(id);
		if (library is null) return new Response { Error = ErrorType.LibNotFound };
		await libRepository.DeleteBook(id, bookId);
		return new Response();
	}

	public async Task<Response> UpdateLibrary(ILibRepository libRepository, Guid id, LibAddRequest req) {
		if (!await libRepository.UpdateLibrary(req, id)) return new Response {Error = ErrorType.LibNotFound};
		return new Response();
	}

	public async Task<Response> DeleteLibrary(ILibRepository libRepository, Guid id) {
		if (!await libRepository.DeleteLibrary(id)) return new Response {Error = ErrorType.LibNotFound};
		return new Response();
	}
}
