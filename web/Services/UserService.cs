using web.Models;
using web.Validation;
using web.Interfaces;
using web.Utilities;
using web.DTO;
using web.Enums;

namespace web.Services;

public class UserService(IUserRepository userRepository, UserAuthReqValidator validator) : IUserService {
	public async Task<Response<User>> Login(UserAuthRequest req) {
		if (!(await validator.ValidateAsync(req)).IsValid) return new Response<User> { Error = ErrorType.InvalidInput };
		
		var user = await userRepository.GetUser(req.Name);
		if (user is null) return new Response<User> {Error = ErrorType.UserNotFound};
		
		return Password.VerifyPassword(req.Password, user.HashedPassword) ? new Response<User> {Data = user} : new Response<User> {Error = ErrorType.WrongPassword};
	}

	public async Task<Response> Register(UserAuthRequest req)
	{
		var validationRes = await validator.ValidateAsync(req);	
		validationRes.Errors.ForEach(Console.WriteLine);
		
		if (!validationRes.IsValid) return new Response { Error = ErrorType.InvalidInput };
		
		var user = await userRepository.GetUser(req.Name);
		if (user != null) return new Response {Error = ErrorType.UserAlreadyExists};

		var newUser = new User(req.Name, Guid.NewGuid(), Password.HashPassword(req.Password), req.Role);
		await userRepository.AddUser(newUser);
		return new Response();
	}

	public async Task<Response> Delete(Guid id) {
		if (!await userRepository.DeleteUser(id)) return new Response { Error = ErrorType.UserNotFound };
		return new Response();
	}

	public async Task<Response<IEnumerable<User>>> GetUsers() {
		var users = await userRepository.GetAll();
		return users.Count == 0
			? new Response<IEnumerable<User>> { Error = ErrorType.NoUsers }
			: new Response<IEnumerable<User>> { Data = users };
	}
}
