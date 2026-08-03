using web.Models;

namespace web.Interfaces;

public interface IUserRepository {
	Task<List<User>> GetAll();
	Task<User?> GetUser(string name);
	Task AddUser(User user);
	Task<bool> DeleteUser(Guid id);
}
