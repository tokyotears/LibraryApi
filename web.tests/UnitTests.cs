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
	[Fact]
	public async Task GetLibrary_LibIsNull_ReturnsLibNotFound() {
		var mock = new Mock<ILibRepository>();
		mock.Setup(rep => rep.GetLibrary(It.IsAny<Guid>())).ReturnsAsync((Library?)null);
		var res = await LibService.GetLibrary(mock.Object, Guid.NewGuid());

		Assert.Equal(ErrorType.LibNotFound, res.Error);
	}

	[Fact]
	public async Task GetLibrary_LibIsNotNull_ReturnsSuccess() {
		var mock = new Mock<ILibRepository>();
		mock.Setup(rep => rep.GetLibrary(It.IsAny<Guid>())).ReturnsAsync(new Library(Guid.NewGuid(), "New", null));
		var res = await LibService.GetLibrary(mock.Object, Guid.NewGuid());

		Assert.True(res.IsSuccess);
	}

	[Fact]
	public async Task GetBooks_LibIsNull_ReturnsLibNotFound() {
		var mock = new Mock<ILibRepository>();
		mock.Setup(rep => rep.GetLibrary(It.IsAny<Guid>())).ReturnsAsync((Library?)null);
		var res = await LibService.GetBooks(mock.Object, Guid.NewGuid());

		Assert.Equal(ErrorType.LibNotFound, res.Error);
	}

	[Fact]
	public async Task GetBooks_LibNotNull_ReturnsSuccess() {
		var mock = new Mock<ILibRepository>();
		mock.Setup(rep => rep.GetLibrary(It.IsAny<Guid>())).ReturnsAsync(new Library(Guid.NewGuid(), "New", null));
		mock.Setup(rep => rep.GetBooks(It.IsAny<Guid>())).ReturnsAsync([]);
		var res = await LibService.GetBooks(mock.Object, Guid.NewGuid());

		Assert.True(res.IsSuccess);
	}

	[Fact]
	public async Task AddLibrary_InvalidInput_ReturnsInvalidInput() {
		var req = new LibAddRequest("", "Desc");
		var mock = new Mock<ILibRepository>();
		var res = await LibService.AddLibrary(mock.Object, Guid.NewGuid(), req);

		Assert.Equal(ErrorType.InvalidInput, res.Error);
	}

	[Fact]
	public async Task AddLibrary_LibExists_ReturnsLibExists() {
		var req = new LibAddRequest("NewLib", "Desc");
		var mock = new Mock<ILibRepository>();
		mock.Setup(rep => rep.GetLibrary(It.IsAny<Guid>())).ReturnsAsync(new Library(Guid.NewGuid(), "NewLib", "Desc"));
		var res = await LibService.AddLibrary(mock.Object, Guid.NewGuid(), req);

		Assert.Equal(ErrorType.LibExists, res.Error);
	}

	[Fact]
	public async Task AddLibrary_LibIsNull_ReturnsSuccess() {
		var req = new LibAddRequest("NewLib", "Desc");
		var mock = new Mock<ILibRepository>();
		mock.Setup(rep => rep.GetLibrary(It.IsAny<Guid>())).ReturnsAsync((Library?) null);
		var res = await LibService.AddLibrary(mock.Object, Guid.NewGuid(), req);

		Assert.True(res.IsSuccess);
	}

	[Fact]
	public async Task AddBook_InvalidInput_ReturnsInvalidInput() {
		var req = new BookAddRequest("", "Desc");
		var mock = new Mock<ILibRepository>();
		var res = await LibService.AddBook(mock.Object, Guid.NewGuid(), req);

		Assert.Equal(ErrorType.InvalidInput, res.Error);
	}

	[Fact]
	public async Task AddBook_LibIsNull_ReturnsLibNotFound() {
		var req = new BookAddRequest("NewLib", "Desc");
		var mock = new Mock<ILibRepository>();
		mock.Setup(rep => rep.GetLibrary(It.IsAny<Guid>())).ReturnsAsync((Library?) null);
		var res = await LibService.AddBook(mock.Object, Guid.NewGuid(), req);

		Assert.Equal(ErrorType.LibNotFound, res.Error);
	}

	[Fact]
	public async Task AddBook_LibNotNull_ReturnsSuccess() {
		var req = new BookAddRequest("NewLib", "Desc");
		var mock = new Mock<ILibRepository>();
		mock.Setup(rep => rep.GetLibrary(It.IsAny<Guid>())).ReturnsAsync(new Library(Guid.NewGuid(), "NewLib", "Desc"));
		var res = await LibService.AddBook(mock.Object, Guid.NewGuid(), req);

		Assert.True(res.IsSuccess);
	}

	[Fact]
	public async Task DeleteBook_LibIsNull_ReturnsLibNotFound() {
		var req = new BookAddRequest("NewLib", "Desc");
		var mock = new Mock<ILibRepository>();
		mock.Setup(rep => rep.GetLibrary(It.IsAny<Guid>())).ReturnsAsync((Library?)null);
		var res = await LibService.AddBook(mock.Object, Guid.NewGuid(), req);

		Assert.Equal(ErrorType.LibNotFound, res.Error);
	}

	[Fact]
	public async Task DeleteBook_LibNotNull_ReturnsSuccess() {
		var req = new BookAddRequest("NewLib", "Desc");
		var mock = new Mock<ILibRepository>();
		mock.Setup(rep => rep.GetLibrary(It.IsAny<Guid>())).ReturnsAsync(new Library(Guid.NewGuid(), "NewLib", "Desc"));
		var res = await LibService.AddBook(mock.Object, Guid.NewGuid(), req);

		Assert.True(res.IsSuccess);
	}
}
