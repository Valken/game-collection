using System;
using System.Linq;
using System.Threading.Tasks;
using GameCollection.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestApp
{
    class GameyGame
    {
        public string Title { get; set; }
    }
    class Program
    {
        public static readonly ILoggerFactory LogFactory = LoggerFactory.Create(builder => builder.AddDebug());
        
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
                        //.UseLazyLoadingProxies()
                        .UseLoggerFactory(LogFactory)
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
                .WithRelatedData()
                .Where(g => g.Name.Contains("Galaxy"))
                .ForEachAsync(g =>
                {
                    Console.WriteLine(g.Name);
                    Console.WriteLine(g.GamesSystems.FirstOrDefault()?.System.Name);
                });

            var gamecubeGames = gamesContext
                .Games
                .WithRelatedData()
                .Where(g => 
                    g.GamesSystems
                        .Any(gs => 
                            gs.System.Name.Contains("Gamecube")))
                .ToArray();
            
            var moreGames = gamesContext
                .Games
                .Select(g =>
                    new
                    {
                        Title = g.Name,
                        System = g.GamesSystems.Select(gs => new 
                        {
                            Name = gs.System.Name, 
                            Region = gs.GamesSystemsReleases.Select(gsr => new 
                            {
                                gsr.Region,
                                gsr.ReleaseDate
                            })
                        })
                    });
            
            var aGame = moreGames.ToArray();
            string.Join(", ", aGame[0].System.Select(s => s.Name));
            foreach (var game in aGame)
            {
                Console.WriteLine($"{game.Title} {string.Join(", ", game.System.Select(s => s.Name))}");
            }


            var stuff = gamesContext
                .Games
                .Where(g => g.Name == "Super Mario Odyssey")
                .Select(g => new GameyGame {Title = g.Name})
                .First();

            stuff.Title = "blah";
            gamesContext.SaveChanges();

            // var marioOdyssey = gamesContext
            //     .Games
            //     .WithRelatedData()
            //     .Single(g => g.Name.Contains("Odyssey"));
            // marioOdyssey
            //     .GamesSystems
            //     .First()
            //     .GamesSystemsReleases
            //     .Add(new GameSystemRelease
            //     {
            //         ReleaseDate = new DateTime(2017, 10, 27)
            //     });
            // await gamesContext.SaveChangesAsync();

            // var newGame = new Game {Name = "Super Mario Odyssey"};
            // newGame.GamesSystems.Add(new GameSystem
            // {
            //     System = gamesContext.Systems.Single(s => s.Name.Contains("Switch"))
            // });
            // gamesContext.Add(newGame);
            // await gamesContext.SaveChangesAsync();


        }
    }
}