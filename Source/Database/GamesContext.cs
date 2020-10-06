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
        public DbSet<System> Systems { get; set; }
        public DbSet<GameSystem> GamesSystems { get; set; }
        public DbSet<GameSystemRelease> GamesSystemsReleases { get; set; }
        //
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Game>(entity =>
        //     {
        //         entity.HasKey(e => e.GameId);
        //         entity.Property(e => e.Name).HasMaxLength(200);
        //     });
        //
        //     modelBuilder.Entity<GameSystem>(entity =>
        //     {
        //         entity.HasOne(d => d.Game)
        //             .WithMany(p => p.GamesSystems)
        //             .HasForeignKey(d => d.GameId);
        //
        //         entity.HasOne(d => d.System)
        //             .WithMany(p => p.GamesSystems)
        //             .HasForeignKey(d => d.SystemId)
        //             .OnDelete(DeleteBehavior.ClientSetNull);
        //     });
        //
        //     modelBuilder.Entity<GameSystemRelease>(entity =>
        //     {
        //         entity.HasOne(d => d.GameSystem)
        //             .WithMany(p => p.GamesSystemsReleases)
        //             .HasForeignKey(d => d.GameSystemId);
        //     });
        //
        //     modelBuilder.Entity<System>(entity =>
        //     {
        //         entity.HasKey(e => e.SystemId);
        //
        //         entity.Property(e => e.Name).HasMaxLength(200);
        //     });
        // }
    }
}
