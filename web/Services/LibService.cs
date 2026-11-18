using FluentValidation;
using web.Interfaces;
using web.Models;
using web.DTO;
using web.Enums;

namespace web.Services;

public class LibService(ILibRepository libRepository, IValidator<LibCreateRequest> libAddValidator, IValidator<BookCreateRequest> bookAddValidator) {
	public async Task<Response<Library>> GetLibrary(Guid userId, Guid libId) {
		var library = await libRepository.GetLibrary(userId, libId);
		return library is null ? new Response<Library> { Error = ErrorType.LibNotFound } : new Response<Library> { Data = library };
	}

	public async Task<Response<List<Library>>> GetMyLibraries(Guid userId)
	{
		var libraries = await libRepository.GetMyLibraries(userId);
		
		return libraries.Count == 0 ? new Response<List<Library>> {Error = ErrorType.NoLibsFound} : new
			Response<List<Library>> { Data = libraries };
	}

	public async Task<Response<IEnumerable<Library>>> GetAll() {
		var libraries = await libRepository.GetLibraries();
		return new Response<IEnumerable<Library>> { Data = libraries };
	}

	public async Task<Response<IEnumerable<Book>>> GetBooks(Guid userId, Guid libId) {
		var library = await libRepository.GetLibrary(userId, libId);
		if (library is null) return new Response<IEnumerable<Book>> { Error = ErrorType.LibNotFound };
		
		var books = await libRepository.GetBooks(libId);
		return new Response<IEnumerable<Book>> { Data = books };
	}

	public async Task<Response> AddLibrary(Guid userId, LibCreateRequest req)
	{
		var validationRes = await libAddValidator.ValidateAsync(req);

		if (!validationRes.IsValid)
			return new Response
				{ Error = ErrorType.InvalidInput, Errors = [.. validationRes.Errors.Select(e => e.ErrorMessage)] };
		
		var library = new Library(Guid.NewGuid(), userId, req.Name, req.Desc, DateTime.UtcNow);
		await libRepository.AddLibrary(library);
		return new Response();
	}

	public async Task<Response> AddBook(Guid userId, Guid libId, BookCreateRequest req) {
		var validationRes = await bookAddValidator.ValidateAsync(req);

		if (!validationRes.IsValid)
			return new Response
				{ Error = ErrorType.InvalidInput, Errors = [.. validationRes.Errors.Select(e => e.ErrorMessage)] };	
		
		var library = await libRepository.GetLibrary(userId, libId);
		if (library is null) return new Response { Error = ErrorType.LibNotFound };
		await libRepository.AddBook(new Book(Guid.NewGuid(), req.Author, req.Title, library.Id));
		return new Response();
	}

	public async Task<Response> DeleteBook(Guid userId, Guid libId, Guid bookId) {
		var library = await libRepository.GetLibrary(userId, libId);
		if (library is null) return new Response { Error = ErrorType.LibNotFound };
		await libRepository.DeleteBook(bookId);
		return new Response();
	}

	public async Task<Response> UpdateLibrary(Guid libId, LibUpdateRequest req) {
		if (!await libRepository.UpdateLibrary(req, libId)) return new Response {Error = ErrorType.LibNotFound};
		return new Response();
	}

	public async Task<Response> DeleteLibrary(Guid id) {
		if (!await libRepository.DeleteLibrary(id)) return new Response {Error = ErrorType.LibNotFound};
		return new Response();
	}
}
