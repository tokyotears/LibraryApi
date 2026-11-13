using Microsoft.Data.Sqlite;
using web.Models;
using web.Interfaces;
using web.Enums;

namespace web.Repositories;

public class UserRepository(IConfiguration config) : IUserRepository
{
	private readonly string _cs = config.GetConnectionString("DefaultConnection")!;

	public async Task<List<User>> GetAll()
	{
		var users = new List<User>();

		await using var conn = new SqliteConnection(_cs);
		await conn.OpenAsync();

		await using var cmd = conn.CreateCommand();
		cmd.CommandText = "SELECT Id, Name, HashedPassword, Role FROM Users";

		await using var reader = await cmd.ExecuteReaderAsync();
		while (await reader.ReadAsync())
		{
			var user = new User(reader.GetString(1), Guid.Parse(reader.GetString(0)), reader.GetString(2),
				Enum.Parse<Roles>(reader.GetString(3)));
			users.Add(user);
		}

		return users;
	}

	public async Task<User?> GetUser(string name)
	{
		await using var conn = new SqliteConnection(_cs);
		await conn.OpenAsync();

		await using var cmd = conn.CreateCommand();
		cmd.CommandText = "SELECT Id, Name, HashedPassword, Role FROM Users WHERE Name = $name LIMIT 1";
		cmd.Parameters.AddWithValue("$name", name);

		await using var reader = await cmd.ExecuteReaderAsync();
		if (!await reader.ReadAsync()) return null;
		
		return new User(reader.GetString(1), Guid.Parse(reader.GetString(0)), reader.GetString(2),
			Enum.Parse<Roles>(reader.GetString(3)));
	}

	public async Task AddUser(User user) 
	{
		await using var conn = new SqliteConnection(_cs);
		await conn.OpenAsync();

		await using var cmd = conn.CreateCommand();
		cmd.CommandText = "INSERT INTO Users(Id, Name, HashedPassword, Role) VALUES ($id, $name, $hash, $role)";
		cmd.Parameters.AddWithValue("$id", user.Id.ToString());
		cmd.Parameters.AddWithValue("$name", user.Name);
		cmd.Parameters.AddWithValue("$hash", user.HashedPassword);
		cmd.Parameters.AddWithValue("$role", user.Role.ToString());

		await cmd.ExecuteNonQueryAsync();
	}

	public async Task<bool> DeleteUser(Guid id)
	{
		await using var conn = new SqliteConnection(_cs);
		await conn.OpenAsync();

		await using var cmd = conn.CreateCommand();
		cmd.CommandText = "DELETE FROM Users WHERE Id = $id";
		cmd.Parameters.AddWithValue("$id", id);

		var affected = await cmd.ExecuteNonQueryAsync();
		return affected > 0;
	}
}
