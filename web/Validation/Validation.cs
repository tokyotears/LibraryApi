namespace web.Validation;

public static class ValidationMethods {
	public static bool IsValidUsername(string name) {
		if (string.IsNullOrEmpty(name)) return false;
		if (name.Length <= 3) return false;
		return true;
	}
	public static bool IsValidPassword(string password) {
		if (string.IsNullOrEmpty(password)) return false;
		return true;
	}
	public static bool IsValidLibName(string name) {
		if (string.IsNullOrEmpty(name)) return false;
		if (name.Length <= 3) return false;
		return true;
	}
	public static bool IsValidLibDesc(string desc) {
		if (string.IsNullOrEmpty(desc)) return false;
		if (desc.Length <= 3 || desc.Length >= 100) return false;
		return true;
	}
	public static bool IsValidBookAuthor(string author) {
		if (string.IsNullOrEmpty(author)) return false;
		if (author.Length <= 3) return false;
		return true;
	}
	public static bool IsValidBookTitle(string title) {
		if (string.IsNullOrEmpty(title)) return false;
		if (title.Length <= 3) return false;
		return true;
	}
}
