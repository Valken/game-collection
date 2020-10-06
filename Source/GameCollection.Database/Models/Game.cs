using System.Collections.Generic;

namespace GameCollection.Database.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<GameSystem> GamesSystems { get; set; } = new HashSet<GameSystem>();
    }
}