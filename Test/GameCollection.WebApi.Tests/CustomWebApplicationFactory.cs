using System.Linq;
using GameCollection.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameCollection.WebApi.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<GamesContext>));
                services.Remove(descriptor);
                
                services.AddDbContext<GamesContext>(options =>
                {
                    options.UseInMemoryDatabase("Games");
                });
            });
        }
    }

    public static class CustomWebApplicationFactoryExtensions
    {
        public static void SeedDatabase(this CustomWebApplicationFactory<Startup> factory)
        {
            var scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<GamesContext>();
            if (db.Database.EnsureCreated())
            {
                SeedData.Initialise(db);
            }
        }
    }
}