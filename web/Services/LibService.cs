using FluentValidation;
using web.Interfaces;
using web.Models;
using web.DTO;
using web.Enums;

namespace web.Services;

public class LibService(ILibRepository libRepository, IRequestValidationDispatcher validationDispatcher) {
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

	public async Task<Response> AddLibrary(Guid userId, LibCreateRequest req)
	{
		var validationRes = await validationDispatcher.ValidateAsync(req);

		if (!validationRes.IsValid)
			return new Response
				{ Error = ErrorType.InvalidInput, Errors = [.. validationRes.Errors.Select(e => e.ErrorMessage)] };
		
		var library = new Library(Guid.NewGuid(), userId, req.Name, req.Desc, DateTime.UtcNow);
		await libRepository.AddLibrary(library);
		return new Response();
	}

	public async Task<Response> UpdateLibrary(Guid libId, Guid userId, LibUpdateRequest req)
	{
		var validationRes = await validationDispatcher.ValidateAsync(req);

		if (!validationRes.IsValid)
			return new Response
				{ Error = ErrorType.InvalidInput, Errors = [.. validationRes.Errors.Select(e => e.ErrorMessage)] };
		
		if (await libRepository.GetLibrary(userId, libId) is null) return new Response { Error = ErrorType.Forbidden };
		
		if (!await libRepository.UpdateLibrary(req, libId)) return new Response {Error = ErrorType.LibNotFound};
		return new Response();
	}

	public async Task<Response> DeleteLibrary(Guid libId, Guid userId) {
		if (await libRepository.GetLibrary(userId, libId) is null) return new Response { Error = ErrorType.Forbidden };
		
		if (!await libRepository.DeleteLibrary(libId)) return new Response {Error = ErrorType.LibNotFound};
		return new Response();
	}
}
