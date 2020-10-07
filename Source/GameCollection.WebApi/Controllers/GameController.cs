using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCollection.Database;
using GameCollection.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCollection.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GamesContext _gamesContext;
        private readonly ILogger<GameController> _logger;

        public GameController(GamesContext gamesContext, ILogger<GameController> logger)
        {
            _gamesContext = gamesContext ?? throw new ArgumentNullException(nameof(gamesContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        [HttpGet]
        public IEnumerable<Game> Get() => _gamesContext.Games;

        [HttpGet("{system}")]
        public async Task<ActionResult<IEnumerable<Game>>> GetBySystem(string system)
        {
            return await _gamesContext
                .Games
                .Where(g => 
                    g.GamesSystems
                        .Any(gs =>
                            gs.System.Name == system))
                .ToListAsync();
        }
    }
}

// How to return from controller action
// https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-3.1

#if false // Response for Game queries should look like this...
{
    "Id": 1,
    "Title": "Super Mario Odyssey",
    "Systems": [
        {
            "Name": "Switch",
            "Release": [
                {
                    "Region": "All",
                    "Date": "2017-10-27"
                }
            ]
        }
    ]
}
#endif