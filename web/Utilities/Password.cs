using BCryptHasher = BCrypt.Net.BCrypt;

namespace web.Utilities;

public static class Password  {
	public static string HashPassword(string password) {
		string hashedPassword = BCryptHasher.HashPassword(password);
		return hashedPassword;
	}

	public static bool VerifyPassword(string password, string hashedPassword) {
		return BCryptHasher.Verify(password, hashedPassword);
	}
}
