using BCryptHasher = BCrypt.Net.BCrypt;

namespace web.Utilities;

public class Utilities {
	public static string Greeting() => "Welcome to your online library!\nPlease log in";

	public static string HashPassword(string password) {
		string hashedPassword = BCryptHasher.HashPassword(password);
		return hashedPassword;
	}

	public static bool VerifyPassword(string password, string hashedPassword) {
		return BCryptHasher.Verify(password, hashedPassword);
	}
}
