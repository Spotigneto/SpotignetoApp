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
        public DbSet<ArtistaEntity> Artisti { get; set; }
        public DbSet<GenereEntity> Generi { get; set; }
        public DbSet<ProfileEntity> Profili { get; set; }
        public DbSet<SottogenereEntity> Sottogeneri { get; set; }

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