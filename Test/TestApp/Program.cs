using System;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
            
            serviceCollection.AddSingleton<IConfiguration>(config);
            serviceCollection.AddDbContext<GamesContext>(options => options.UseSqlServer("Name=Games"));
            var services = serviceCollection.BuildServiceProvider();
            var gamesContext = services.GetService<GamesContext>();

            await gamesContext.Games.AddRangeAsync(new[]
            {
                new Game { Name = "Super Mario 64" },
                new Game { Name = "Super Mario Sunshine" },
                new Game { Name = "Super Mario Galaxy" }
            });
            await gamesContext.SaveChangesAsync();
            
            var games = gamesContext.Games.ToList();
            Console.ReadKey();
        }
    }
}
