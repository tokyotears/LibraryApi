using Microsoft.Data.Sqlite;
using web.Interfaces;
using web.Models;

namespace web.Repositories;

public class BookRepository(IConfiguration config) : IBookRepository
{
    private readonly string _cs = config.GetConnectionString("DefaultConnection")!;

    public async Task<Book?> GetBook(Guid bookId)
    {
        await using var conn = new SqliteConnection(_cs);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Books WHERE Id = $bookId LIMIT 1";
        cmd.Parameters.AddWithValue("$bookId", bookId);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (! await reader.ReadAsync()) return null;

        return new Book(Guid.Parse(reader.GetString(0)), reader.GetString(1), reader.GetString(2),
            Guid.Parse(reader.GetString(3)));
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

    public async Task UpdateBook(Book book)
    {
        await using var conn = new SqliteConnection(_cs);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();

        var updates = new List<string>();
        if (!string.IsNullOrEmpty(book.Author)) updates.Add("Author = $author"); 
        if (!string.IsNullOrEmpty(book.Title)) updates.Add("Title = $title");
        
        cmd.CommandText = $"UPDATE Books SET {string.Join(' ', updates)} WHERE Id = $bookId";
        if (!string.IsNullOrEmpty(book.Author)) cmd.Parameters.AddWithValue("$author", book.Author);
        if (!string.IsNullOrEmpty(book.Title)) cmd.Parameters.AddWithValue("$title", book.Title);
        cmd.Parameters.AddWithValue("$bookId", book.Id);

        await cmd.ExecuteNonQueryAsync();
    }
    
    public async Task DeleteBook(Guid bookId) 
    {
        await using var conn = new SqliteConnection(_cs);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Books WHERE Id = $id LIMIT 1";
        cmd.Parameters.AddWithValue("$id", bookId);

        await cmd.ExecuteNonQueryAsync();
    }
}