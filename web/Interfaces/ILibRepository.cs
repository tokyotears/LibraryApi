using web.Models;
using web.DTO;

namespace web.Interfaces;

public interface ILibRepository {
	Task<List<Library>> GetLibraries();
	Task<List<Library>> GetMyLibraries(Guid id);
	Task<Library?> GetLibrary(Guid userId, Guid id);
	Task AddLibrary(Library library);
	Task<bool> DeleteLibrary(Guid id);
	Task<bool> UpdateLibrary(LibUpdateRequest req, Guid id);
}
