using Microsoft.Data.Sqlite;
using web.Models;
using web.Interfaces;
using web.DTO;

namespace web.Repositories;

public class LibRepository(IConfiguration config) : ILibRepository
{
	private readonly string _cs = config.GetConnectionString("DefaultConnection")!;

	public async Task<List<Library>> GetLibraries()
	{
		var libs = new List<Library>();
		
		await using var conn = new SqliteConnection(_cs);
		await conn.OpenAsync();

		await using var cmd = conn.CreateCommand();
		cmd.CommandText = "SELECT * FROM Libraries";

		await using var reader = await cmd.ExecuteReaderAsync();

		while (await reader.ReadAsync())
		{
			var lib = new Library(Guid.Parse(reader.GetString(0)), Guid.Parse(reader.GetString(1)), reader.GetString(2), reader.GetString(3), DateTime.Parse(reader.GetString(4)));
			libs.Add(lib);
		}

		return libs;
	}

	public async Task<List<Library>> GetMyLibraries(Guid userId)
	{
		var libs = new List<Library>(); 
		
		await using var conn = new SqliteConnection(_cs);
		await conn.OpenAsync();

		await using var cmd = conn.CreateCommand();
		cmd.CommandText = "SELECT Id, UserId, Name, Desc, CreatedAt FROM Libraries WHERE UserId = $id";
		cmd.Parameters.AddWithValue("$id", userId.ToString());

		await using var reader = await cmd.ExecuteReaderAsync();

		while (await reader.ReadAsync())
		{
			var lib = new Library(Guid.Parse(reader.GetString(0)), Guid.Parse(reader.GetString(1)), reader.GetString(2), reader.GetString(3),
				DateTime.Parse(reader.GetString(4)));
			libs.Add(lib);
		}

		return libs;
	}

	public async Task<Library?> GetLibrary(Guid userId, Guid libId)
	{
		await using var conn = new SqliteConnection(_cs);
		await conn.OpenAsync();

		await using var cmd = conn.CreateCommand();
		cmd.CommandText = "SELECT * FROM Libraries WHERE UserId = $userId, Id = $id LIMIT 1";
		cmd.Parameters.AddWithValue("$userId", userId.ToString());
		cmd.Parameters.AddWithValue("$id", libId.ToString());

		await using var reader = await cmd.ExecuteReaderAsync();
		if (!await reader.ReadAsync()) return null;

		return new Library(Guid.Parse(reader.GetString(0)), Guid.Parse(reader.GetString(1)), reader.GetString(2),
			reader.GetString(3), DateTime.Parse(reader.GetString(4)));
	}

	public async Task AddLibrary(Library library)
	{
		await using var conn = new SqliteConnection(_cs);
		await conn.OpenAsync();

		await using var cmd = conn.CreateCommand();
		cmd.CommandText =
			"INSERT INTO Libraries(Id, UserId, Name, Desc, CreatedAt) VALUES ($id, $userId, $name, $desc, $createdAt)";
		cmd.Parameters.AddWithValue("$id", library.Id);
		cmd.Parameters.AddWithValue("$userId", library.UserId);
		cmd.Parameters.AddWithValue("$name", library.Name);
		cmd.Parameters.AddWithValue("$desc", library.Desc);
		cmd.Parameters.AddWithValue("$createdAt", library.CreatedAt);

		await cmd.ExecuteNonQueryAsync();
	}

	public async Task<bool> DeleteLibrary(Guid libId)
	{
		await using var conn = new SqliteConnection(_cs);
		await conn.OpenAsync();

		await using var cmd = conn.CreateCommand();
		cmd.CommandText = "DELETE FROM Libraries WHERE Id = $id";
		cmd.Parameters.AddWithValue("$id", libId);

		var affected = await cmd.ExecuteNonQueryAsync();
		return affected > 0;
	}

	public async Task<List<Book>> GetBooks(Guid libId)
	{
		var books = new List<Book>();
		await using var conn = new SqliteConnection(_cs);
		await conn.OpenAsync();

		await using var cmd = conn.CreateCommand();
		cmd.CommandText = "SELECT * FROM Books WHERE LibId = $libId";
		cmd.Parameters.AddWithValue("$libId", libId);

		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			var book = new Book(Guid.Parse(reader.GetString(0)), reader.GetString(1), reader.GetString(2),
				Guid.Parse(reader.GetString(3)));
			books.Add(book);
		}

		return books;
	}

	public async Task AddBook(Book book) 
	{
		await using var conn = new SqliteConnection(_cs);
		await conn.OpenAsync();

		await using var cmd = conn.CreateCommand();
		cmd.CommandText = "INSERT INTO Books(Id, Author, Title, LibId) VALUES ($id, $author, $title, $libId)";
		cmd.Parameters.AddWithValue("$id", book.Id);
		cmd.Parameters.AddWithValue("$author", book.Author);	
		cmd.Parameters.AddWithValue("$title", book.Title);
		cmd.Parameters.AddWithValue("$libId", book.LibId);

		await cmd.ExecuteNonQueryAsync();
	}

	public async Task<bool> DeleteBook(Guid bookId) 
	{
		await using var conn = new SqliteConnection(_cs);
		await conn.OpenAsync();

		await using var cmd = conn.CreateCommand();
		cmd.CommandText = "DELETE FROM Books WHERE Id = $id LIMIT 1";
		cmd.Parameters.AddWithValue("$id", bookId);

		var affected = await cmd.ExecuteNonQueryAsync();

		return affected > 0;
	}

	public async Task<bool> UpdateLibrary(LibAddRequest req, Guid libId)
	{
		await using var conn = new SqliteConnection(_cs);
		await conn.OpenAsync();

		await using var cmd = conn.CreateCommand();
		cmd.CommandText = "UPDATE Libraries SET Name = $name, Desc = $desc WHERE Id = $id";
		cmd.Parameters.AddWithValue("$id", libId);
		cmd.Parameters.AddWithValue("$name", req.Name);
		cmd.Parameters.AddWithValue("$desc", req.Desc);

		var affected = await cmd.ExecuteNonQueryAsync();

		return affected > 0;
	}
}
