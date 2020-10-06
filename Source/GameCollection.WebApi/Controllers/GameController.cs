using System;
using System.Collections.Generic;
using System.Linq;
using GameCollection.Database;
using GameCollection.Database.Models;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<IEnumerable<Game>> Get()
        {
            _logger.LogInformation("Fetching games from {Context}", _gamesContext);
            return _gamesContext.Games;
        }
    }
}

// How to return from controller action
// https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-3.1