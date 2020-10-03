using System;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class GamesContext : DbContext
    {
        public GamesContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Game> Games { get; set; }
    }
}
