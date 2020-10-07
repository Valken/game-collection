using System;
using System.Collections.Generic;
using System.Linq;
using GameCollection.Database;
using GameCollection.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCollection.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemController : ControllerBase
    {
        private readonly GamesContext _gamesContext;
        private readonly ILogger<SystemController> _logger;

        public SystemController(GamesContext gamesContext, ILogger<SystemController> logger)
        {
            _gamesContext = gamesContext ?? throw new ArgumentNullException(nameof(gamesContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IEnumerable<GameCollection.Database.Models.System> List() => _gamesContext.Systems;

        [HttpGet("{id}")]
        public GameCollection.Database.Models.System Get(int id) =>
            _gamesContext.Systems.SingleOrDefault(g => g.SystemId == id);

        [HttpPost]
        public void Create([FromBody] GameCollection.Database.Models.System system)
        {
            _gamesContext.Add(system);
            _gamesContext.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var system = _gamesContext.Systems.Find(id);
            _gamesContext.Remove(system);
            _gamesContext.SaveChanges();
        }

        [HttpPut("{id}")]
        public void Update(int id, [FromBody]GameCollection.Database.Models.System system)
        {
            throw new ArgumentNullException(nameof(Update));
        }
    }
}