using web.Models;
using web.DTO;

namespace web.Interfaces;

public interface IUserService {
	Task<Response<User>> Login(IUserRepository userRepository, UserAuthRequest req);
	Task<Response> Register(IUserRepository userRepository, UserAuthRequest req);
	Task<Response> Delete(IUserRepository userRepository, Guid id);
	Task<Response<IEnumerable<User>>> GetUsers(IUserRepository userRepository);
}
