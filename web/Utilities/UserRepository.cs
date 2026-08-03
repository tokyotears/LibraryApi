using web.Models;
using web.Data;
using web.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace web.Utilities;

public class UserRepository(AppDbContext db) : IUserRepository {
	public async Task<List<User>> GetAll() => await db.Users.AsNoTracking().ToListAsync();

	public async Task<User?> GetUser(string name) => await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Name == name);

	public async Task AddUser(User user) {
		db.Users.Add(user);
		await db.SaveChangesAsync();
	}

	public async Task<bool> DeleteUser(Guid id) {
		User? user = await db.Users.FindAsync(id);
		if (user == null) return false;
		db.Users.Remove(user);
		await db.SaveChangesAsync();
		return true;
	}
}
