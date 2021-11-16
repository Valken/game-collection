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
                    options.EnableSensitiveDataLogging();
                });
            });
        }
    }
}