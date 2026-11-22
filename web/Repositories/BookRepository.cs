using Microsoft.Data.Sqlite;
using web.DTO;
using web.Interfaces;
using web.Models;
using web.Enums;

namespace web.Repositories;

public class BookRepository(IConfiguration config) : IBookRepository
{
    private readonly string _cs = config.GetConnectionString("DefaultConnection")!;

    public async Task<Book?> GetBook(Guid userId, Guid libId, Guid bookId)
    {
        await using var conn = new SqliteConnection(_cs);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT 1 FROM Books WHERE Id = $bookId AND LibId = $libId AND EXISTS (SELECT 1 FROM Libraries WHERE Libraries.Id = $libId AND Libraries.UserId = $userId)";
        cmd.Parameters.AddWithValue("$userId", userId);
        cmd.Parameters.AddWithValue("$libId", libId); 
        cmd.Parameters.AddWithValue("$bookId", bookId);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (! await reader.ReadAsync()) return null;

        return new Book(Guid.Parse(reader.GetString(0)), reader.GetString(1), reader.GetString(2),
            Guid.Parse(reader.GetString(3)));
    }

    public async Task<List<Book>> GetBooks(Guid userId, Guid libId)
    {
        var books = new List<Book>();
        await using var conn = new SqliteConnection(_cs);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Books WHERE LibId = $libId AND EXISTS (SELECT 1 FROM Libraries WHERE Libraries.Id = $libId and Libraries.UserId = $userId)";
        cmd.Parameters.AddWithValue("$userId", userId);
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
    
    public async Task<RepoStatus> AddBook(Guid userId, Guid libId, BookCreateRequest req) 
    {
        await using var conn = new SqliteConnection(_cs);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();

        cmd.CommandText = "INSERT INTO Books(Id, Author, Title, LibId) SELECT $id, $author, $title, $libId WHERE EXISTS (SELECT 1 FROM Libraries WHERE Id = $libId AND UserId = $userId)";
        cmd.Parameters.AddWithValue("$id", Guid.NewGuid());
        cmd.Parameters.AddWithValue("$author", req.Author);
        cmd.Parameters.AddWithValue("$title", req.Title);
        cmd.Parameters.AddWithValue("$userId", userId);
        cmd.Parameters.AddWithValue("$libId", libId);

        var affected = await cmd.ExecuteNonQueryAsync();

        return affected > 0 ? RepoStatus.Success : RepoStatus.NotFound;
    }

    public async Task<RepoStatus> UpdateBook(Guid userId, Guid libId, Guid bookId, BookUpdateRequest req)
    {
        await using var conn = new SqliteConnection(_cs);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();

        var updates = new List<string>();
        if (req.Author is not null) updates.Add("Author = $author"); 
        if (req.Title is not null) updates.Add("Title = $title");

        cmd.CommandText = $"UPDATE Books SET {string.Join(", ", updates)} WHERE Id = $bookId AND LibID = $libId AND EXISTS (SELECT 1 FROM Libraries WHERE Libraries.Id = $libId AND Libraries.UserId = $userId)";

        if (req.Author is not null) cmd.Parameters.AddWithValue("$author", req.Author);
        if (req.Title is not null) cmd.Parameters.AddWithValue("$title", req.Title);

        cmd.Parameters.AddWithValue("$userId", userId);
        cmd.Parameters.AddWithValue("$libId", libId);
        cmd.Parameters.AddWithValue("$bookId", bookId);

        var affected = await cmd.ExecuteNonQueryAsync();

        return affected == 0 ? RepoStatus.NotFound : RepoStatus.Success;
    }
    
    public async Task<RepoStatus> DeleteBook(Guid userId, Guid libId, Guid bookId) 
    {
        await using var conn = new SqliteConnection(_cs);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();

        cmd.CommandText =
            "DELETE * FROM Books WHERE Id = $bookId AND LibId = $libId AND EXISTS (SELECT 1 FROM Libraries WHERE Libraries.Id = $libId AND Libraries.UserId = $userId) LIMIT 1";

        cmd.Parameters.AddWithValue("$userId", userId);
        cmd.Parameters.AddWithValue("$libId", libId);
        cmd.Parameters.AddWithValue("$bookId", bookId);

        var affected = await cmd.ExecuteNonQueryAsync();

        return affected == 0 ? RepoStatus.NotFound : RepoStatus.Success;
    }
}