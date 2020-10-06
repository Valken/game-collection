using System.Collections.Generic;

namespace GameCollection.Database.Models
{
    public class GameSystem
    {
        public int GameSystemId { get; set; }
        public int GameId { get; set; }
        public int SystemId { get; set; }

        public virtual Game Game { get; set; }
        public virtual System System { get; set; }
        public virtual ICollection<GameSystemRelease> GamesSystemsReleases { get; set; } = new HashSet<GameSystemRelease>();
    }
}