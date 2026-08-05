using System.Net;
using System.Net.Http.Json;
using web.DTO;
using web.Models;

namespace web.Tests;

public class UserEndpointsTests {

	[Fact]
	public async Task Register_Login_ReturnsSuccess() {
		var factory = new TestWebApplicationFactory();
		HttpClient _client = factory.CreateClient();

		var req = new UserAuthRequest("Pasha", "12345");
		var resRegister = await _client.PostAsJsonAsync("/register", req);
		Assert.Equal(HttpStatusCode.OK, resRegister.StatusCode);

		var resLogin = await _client.PostAsJsonAsync("/login", req);
		Assert.Equal(HttpStatusCode.OK, resLogin.StatusCode);
	}

	[Fact]
	public async Task Register_Login_Logout_ReturnsSuccess() {
		var factory = new TestWebApplicationFactory();
		HttpClient _client = factory.CreateClient();

		var req = new UserAuthRequest("Pasha", "12345");
		var resRegister = await _client.PostAsJsonAsync("/register", req);
		Assert.Equal(HttpStatusCode.OK, resRegister.StatusCode);

		var resLogin = await _client.PostAsJsonAsync("/login", req);
		Assert.Equal(HttpStatusCode.OK, resLogin.StatusCode);

		var resUsers = await _client.GetFromJsonAsync<List<User>>("/users");
		Assert.NotNull(resUsers);

		var resLogout = await _client.PostAsJsonAsync("/logout", req);
		Assert.Equal(HttpStatusCode.OK, resLogout.StatusCode);
	}
}
