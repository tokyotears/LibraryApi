using web.Models;
using web.DTO;

namespace web.Interfaces;

public interface IUserService {
	Task<Response<User>> Login(UserAuthRequest req);
	Task<Response> Register(UserAuthRequest req);
	Task<Response> Delete(Guid id);
	Task<Response<IEnumerable<User>>> GetUsers();
}
