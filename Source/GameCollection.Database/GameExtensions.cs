using System.Linq;
using GameCollection.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCollection.Database
{
    public static class GameExtensions
    {
        public static IQueryable<Game> WithRelatedData(this IQueryable<Game> games)
        {
            return games
                .Include(g => g.GamesSystems)
                .ThenInclude(gs => gs.System)
                .Include(g => g.GamesSystems)
                .ThenInclude(gs => gs.GamesSystemsReleases);
        }
    }
}