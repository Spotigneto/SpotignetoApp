using Microsoft.EntityFrameworkCore;
using Backend.Entities;

namespace Backend.Data
{
    public class SpotigneteDbContext : DbContext
    {
        public SpotigneteDbContext(DbContextOptions<SpotigneteDbContext> options) : base(options)
        {
        }

        public DbSet<CanzoneEntity> Canzoni { get; set; }
        public DbSet<PlaylistEntity> Playlists { get; set; }
        public DbSet<AlbumEntity> Albums { get; set; }
        public DbSet<ArtistaEntity> Artisti { get; set; }
        public DbSet<ProfileEntity> Utenti { get; set; }

        public DbSet<AsCanzonePlaylistEntity> AsCanzonePlaylist { get; set; }
        public DbSet<AsAlbumCanzoneEntity> AsAlbumCanzone { get; set; }
        public DbSet<AsUtentePlaylistEntity> AsUtentePlaylist { get; set; }
        public DbSet<AsArtistaAlbumEntity> AsArtistaAlbum { get; set; }
        public DbSet<AsArtistaCanzoneEntity> AsArtistaCanzone { get; set; }
        public DbSet<AsUtenteArtistaEntity> AsUtenteArtista { get; set; }

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