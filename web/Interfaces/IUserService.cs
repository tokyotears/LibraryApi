using web.Models;
using web.DTO;
using web.Validation;

namespace web.Interfaces;

public interface IUserService {
	Task<Response<User>> Login(IUserRepository userRepository, UserAuthReqValidator validator, UserAuthRequest req);
	Task<Response> Register(IUserRepository userRepository, UserAuthReqValidator validator, UserAuthRequest req);
	Task<Response> Delete(IUserRepository userRepository, Guid id);
	Task<Response<IEnumerable<User>>> GetUsers(IUserRepository userRepository);
}
