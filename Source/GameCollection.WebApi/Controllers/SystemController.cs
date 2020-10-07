using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCollection.Database;
using Microsoft.AspNetCore.Mvc;
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
        public  ActionResult<IEnumerable<Database.Models.System>> List() => 
            _gamesContext.Systems;

        [HttpGet("{id}")]
        public Database.Models.System Get(int id) =>
            _gamesContext.Systems.SingleOrDefault(g => g.SystemId == id);

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Database.Models.System system)
        {
            _logger.LogInformation("Adding {System}", system.Name);
            _gamesContext.Add(system);
            await _gamesContext.SaveChangesAsync();

            // https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-3.1
            // Returns the created object in the body, returns the URL of the created object in a 
            // Location header. Is that normal?
            return CreatedAtAction(nameof(Get), new { id = system.SystemId }, system);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var system = await _gamesContext.Systems.FindAsync(id);
            if (system == null)
            {
                return NotFound();
            }
            
            _gamesContext.Remove(system);
            await _gamesContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public void Update(int id, Database.Models.System system)
        {
            throw new ArgumentNullException(nameof(Update));
        }
    }
}