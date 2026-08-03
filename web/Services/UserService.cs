using web.Models;
using web.Validation;
using web.Interfaces;
using U = web.Utilities.Utilities;
using web.DTO;

namespace web.Services;

public static class UserService {
	public async static Task<Response<User>> Login(IUserRepository userRepository, UserAuthRequest req) {
		if (!ValidationMethods.IsValidUsername(req.Name) || !ValidationMethods.IsValidPassword(req.Password)) {
			return new Response<User> { Error = ErrorType.InvalidInput };
		}
		User? user = await userRepository.GetUser(req.Name);
		if (user is null) return new Response<User> {Error = ErrorType.UserNotFound};
		if (!U.VerifyPassword(req.Password, user.HashedPassword)) return new Response<User> {Error = ErrorType.WrongPassword};
		return new Response<User> { Data = user };
	}

	public async static Task<Response> Register(IUserRepository userRepository, UserAuthRequest req) {
		if (!ValidationMethods.IsValidUsername(req.Name) || !ValidationMethods.IsValidPassword(req.Password)) {
			return new Response { Error = ErrorType.InvalidInput };
		}
		var user = await userRepository.GetUser(req.Name);
		if (user != null) {
			return new Response {Error = ErrorType.UserAlreadyExists};
		} else {
			User newUser = new (req.Name, Guid.NewGuid(), U.HashPassword(req.Password));
			await userRepository.AddUser(newUser);
			return new Response();
		}
	}

	public async static Task<Response> Delete(IUserRepository userRepository, Guid id) {
		if (!await userRepository.DeleteUser(id)) {
			return new Response { Error = ErrorType.UserNotFound };
		} else return new Response();
	}

	public async static Task<Response<IEnumerable<User>>> GetUsers(IUserRepository userRepository) {
		var users = await userRepository.GetAll();
		if (users.Count == 0) return new Response<IEnumerable<User>> { Error = ErrorType.NoUsers };
		return new Response<IEnumerable<User>> { Data = users };
	}
}
