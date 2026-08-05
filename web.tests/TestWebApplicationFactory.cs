using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using web.Data;

namespace web.Tests;

public class TestWebApplicationFactory : WebApplicationFactory<Program> {
	private SqliteConnection _connection = null!;
	protected override void ConfigureWebHost(IWebHostBuilder builder) {
		builder.ConfigureServices(services => {
			services.RemoveAll<AppDbContext>();
			_connection = new SqliteConnection("DataSource=:memory:");
			_connection.Open();

			services.AddDbContext<AppDbContext>(options => {
				options.UseSqlite(_connection);
			});
			var serviceProvider = services.BuildServiceProvider();
			using var scope = serviceProvider.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
		});
	}
}
