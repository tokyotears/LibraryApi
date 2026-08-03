using web.Services;
using web.DTO;
using web.Interfaces;
using web.Models;
using Moq;

namespace web.Tests;

public class UserServiceTests {
	private readonly string hash = "$2a$11$S4DQcrK9sEZwZESQIUXu2.zmCnZHK6BMTaEyYKRlj1CTdcBUx6e1y";

	[Fact]
	public async Task Register_InvalidInput_ReturnsInvalidInput() {
		var req = new UserAuthRequest("Pasha", "");

		var mock = new Mock<IUserRepository>();
		var res = await UserService.Register(mock.Object, req);

		Assert.Equal(ErrorType.InvalidInput, res.Error);
	}

	[Fact]
	public async Task Register_UserNotNull_ReturnsUserAlreadyExists() {
		var req = new UserAuthRequest("Pasha", "12345");

		var mock = new Mock<IUserRepository>();
		mock.Setup(rep => rep.GetUser(It.IsAny<string>())).ReturnsAsync(new User(req.Name, Guid.NewGuid(), req.Password));
		var res = await UserService.Register(mock.Object, req);

		Assert.Equal(ErrorType.UserAlreadyExists, res.Error);
	}

	[Fact]
	public async Task Register_UserIsNull_ReturnsSuccess() {
		var req = new UserAuthRequest("Pasha", "12345");

		var mock = new Mock<IUserRepository>();
		mock.Setup(rep => rep.GetUser(It.IsAny<string>())).ReturnsAsync((User?) null);
		var res = await UserService.Register(mock.Object, req);

		Assert.True(res.IsSuccess);
	}

	[Fact]
	public async Task Login_InvalidInput_ReturnsInvalidInput() {
		var req = new UserAuthRequest("Pasha", "");

		var mock = new Mock<IUserRepository>();
		var res = await UserService.Register(mock.Object, req);

		Assert.Equal(ErrorType.InvalidInput, res.Error);
	}

	[Fact]
	public async Task Login_UserIsNull_ReturnsUserNotFound() {
		var req = new UserAuthRequest("Pasha", "12345");

		var mock = new Mock<IUserRepository>();
		mock.Setup(rep => rep.GetUser(It.IsAny<string>())).ReturnsAsync((User?) null);
		var res = await UserService.Login(mock.Object, req);

		Assert.Equal(ErrorType.UserNotFound, res.Error);
	}

	[Fact]
	public async Task Login_WrongPassword_ReturnsWrongPassword() {
		var req = new UserAuthRequest("Pasha", "12345");

		var mock = new Mock<IUserRepository>();
		mock.Setup(rep => rep.GetUser(It.IsAny<string>())).ReturnsAsync(new User(req.Name, Guid.NewGuid(), hash));
		var res = await UserService.Login(mock.Object, req);

		Assert.Equal(ErrorType.WrongPassword, res.Error);
	}

	[Fact]
	public async Task Login_Ok_ReturnsSuccess() {
		var req = new UserAuthRequest("Pasha", "qwerty");

		var mock = new Mock<IUserRepository>();
		mock.Setup(rep => rep.GetUser(It.IsAny<string>())).ReturnsAsync(new User(req.Name, Guid.NewGuid(), hash));
		var res = await UserService.Login(mock.Object, req);

		Assert.True(res.IsSuccess);
	}

	[Fact]
	public async Task Delete_UserIsNull_ReturnsUserNotFound() {
		var mock = new Mock<IUserRepository>();
		mock.Setup(rep => rep.DeleteUser(It.IsAny<Guid>())).ReturnsAsync(false);
		var res = await UserService.Delete(mock.Object, Guid.NewGuid());

		Assert.Equal(ErrorType.UserNotFound, res.Error);
	}

	[Fact]
	public async Task Delete_UserNotNull_ReturnsOk() {
		var mock = new Mock<IUserRepository>();
		mock.Setup(rep => rep.DeleteUser(It.IsAny<Guid>())).ReturnsAsync(true);
		var res = await UserService.Delete(mock.Object, Guid.NewGuid());

		Assert.True(res.IsSuccess);
	}

	[Fact]
	public async Task GetUsers_UsersIsNull_ReturnsNoUsers() {
		var mock = new Mock<IUserRepository>();
		mock.Setup(rep => rep.GetAll()).ReturnsAsync([]);
		var res = await UserService.GetUsers(mock.Object);

		Assert.Equal(ErrorType.NoUsers, res.Error);
	}

	[Fact]
	public async Task GetUsers_UsersNotNull_ReturnsSuccess() {
		var mock = new Mock<IUserRepository>();
		mock.Setup(rep => rep.GetAll()).ReturnsAsync([new User("Pasha", Guid.NewGuid(), "1234")]);
		var res = await UserService.GetUsers(mock.Object);

		Assert.True(res.IsSuccess);
	}
}

public class LibServiceTests {

}
