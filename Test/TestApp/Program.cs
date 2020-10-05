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
            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
            
            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddDbContext<GamesContext>(options => 
                    options
                        // Be warned, this requires multiple active result sets on connection string!
                        // Probably better to .Include the dependent tables...
                        .UseLazyLoadingProxies() 
                        .UseSqlServer("Name=Games"))
                .BuildServiceProvider();
            
            var gamesContext = services.GetService<GamesContext>();

            // await gamesContext.Games.AddRangeAsync(new[]
            // {
            //     new Game { Name = "Super Mario 64" },
            //     new Game { Name = "Super Mario Sunshine" },
            //     new Game { Name = "Super Mario Galaxy" }
            // });
            // await gamesContext.SaveChangesAsync();

            await gamesContext
                .Games
                .Where(g => g.Name.Contains("Galaxy"))
                // .Include(g => g.GamesSystems)
                // .ThenInclude(gs => gs.GamesSystemsReleases)
                .ForEachAsync(g =>
                {
                    Console.WriteLine(g.Name);
                    Console.WriteLine(g.GamesSystems.FirstOrDefault()?.System.Name);
                });
        }
    }
}