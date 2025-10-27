using Microsoft.EntityFrameworkCore;
using Backend.Entities;

namespace Backend.Data
{
    public class SpotigneteDbContext : DbContext
    {
        public SpotigneteDbContext(DbContextOptions<SpotigneteDbContext> options) : base(options)
        {
        }

        // Entity DbSets
        public DbSet<CanzoneEntity> Canzoni { get; set; }
        public DbSet<PlaylistEntity> Playlists { get; set; }
        public DbSet<AlbumEntity> Albums { get; set; }
        public DbSet<AsCanzonePlaylistEntity> AsCanzonePlaylist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure your entities here
            // Example:
            // modelBuilder.Entity<Track>()
            //     .HasKey(t => t.Id);
        }
    }
}