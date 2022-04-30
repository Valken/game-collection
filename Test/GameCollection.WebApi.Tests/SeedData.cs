using GameCollection.Database;
using GameCollection.Database.Models;

namespace GameCollection.WebApi.Tests;

public static class SeedData
{
    public static void Initialise(GamesContext database)
    {
        var nintendo64 = new Database.Models.System { Name = "Nintendo 64" };
        var gamecube = new Database.Models.System { Name = "Gamecube" };
        var superMario64 = new Game { Name = "Super Mario 64" };
        var release = new GameSystemRelease { GameSystem = new GameSystem { Game = superMario64, System = nintendo64 }, Region = "NTSC-J",};
        //database.Systems.AddRange(nintendo64);
        database.GamesSystemsReleases.Add(release);
        database.SaveChanges();
    }
}