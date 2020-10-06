using System;

namespace Database
{
    public class GameSystemRelease
    {
        public int GameSystemReleaseId { get; set; }
        public int GameSystemId { get; set; }
        public string Region { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public virtual GameSystem GameSystem { get; set; } 
    }
}