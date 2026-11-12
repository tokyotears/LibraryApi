using Microsoft.EntityFrameworkCore;
using web.Models;
using web.DataConfigs;

namespace web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
	public DbSet<User> Users => Set<User>();
	public DbSet<Library> Libraries => Set<Library>();
	public DbSet<Book> Books => Set<Book>();

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.ApplyConfiguration(new UserConfiguration());
		modelBuilder.ApplyConfiguration(new LibConfiguration());
		modelBuilder.ApplyConfiguration(new BookConfiguration());
	}
}
