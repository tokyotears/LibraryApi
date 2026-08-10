using web.Models;
using web.Data;
using web.Interfaces;
using web.DTO;
using Microsoft.EntityFrameworkCore;

namespace web.Utilities;

public class LibRepository(AppDbContext db) : ILibRepository {
	public async Task<List<Library>> GetLibraries() => await db.Libraries.AsNoTracking().ToListAsync();

	public async Task<Library?> GetLibrary(Guid id) => await db.Libraries.FindAsync(id);

	public async Task AddLibrary(Library library) {
		db.Libraries.Add(library);
		await db.SaveChangesAsync();
	}

	public async Task<bool> DeleteLibrary(Guid id) {
		var library = await db.Libraries.FindAsync(id);
		if (library is null) return false;
		db.Libraries.Remove(library);
		await db.SaveChangesAsync();
		return true;
	}

	public async Task<List<Book>> GetBooks(Guid id) => await db.Books.Where(b => b.LibId == id).AsNoTracking().ToListAsync();

	public async Task AddBook(Guid id, string author, string title) {
		Book book = new(author, title, id);
		db.Books.Add(book);
		await db.SaveChangesAsync();
	}

	public async Task<bool> DeleteBook(Guid id, Guid bookId) {
		var book = await db.Books.FindAsync(bookId);
		if (book is null || book.LibId != id) return false;
		db.Books.Remove(book);
		await db.SaveChangesAsync();
		return true;
	}

	public async Task<bool> UpdateLibrary(LibAddRequest req, Guid id) {
		var lib = await GetLibrary(id);
		if (lib is null) return false;
		lib.Name = req.Name;
		lib.Desc = req.Desc;
		await db.SaveChangesAsync();
		return true;
	}
}
