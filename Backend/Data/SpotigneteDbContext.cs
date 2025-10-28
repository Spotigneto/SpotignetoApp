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

    // Compatibility DbSets / simplified entity shapes used across services/controllers
    // These types provide simpler property names (Id, Nome, etc.) expected by the rest of the codebase.
    public DbSet<Canzone> Canzone { get; set; }
    public DbSet<Playlist> Playlist { get; set; }
    public DbSet<Artista> Artista { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure your entities here
            // Example:
            // modelBuilder.Entity<Track>()
            //     .HasKey(t => t.Id);

            // Map the simplified shapes onto the existing tables/columns
            modelBuilder.Entity<Canzone>(b =>
            {
                b.ToTable("Canzone");
                b.HasKey(e => e.Id);
                b.Property(e => e.Id).HasColumnName("ca_id");
                b.Property(e => e.Nome).HasColumnName("ca_nome").HasMaxLength(100);
                b.Property(e => e.File).HasColumnName("ca_file").HasMaxLength(500);
                b.Property(e => e.GenereFk).HasColumnName("ca_genere_fk");
                b.Property(e => e.SottogenereFk).HasColumnName("ca_sottogenere_fk");
                b.Property(e => e.Durata).HasColumnName("ca_durata").HasMaxLength(10);
                b.Property(e => e.Secondi).HasColumnName("ca_secondi");
            });

            modelBuilder.Entity<Playlist>(b =>
            {
                b.ToTable("Playlist");
                b.HasKey(e => e.Id);
                b.Property(e => e.Id).HasColumnName("pl_id");
                b.Property(e => e.Nome).HasColumnName("pl_nome").HasMaxLength(100);
                b.Property(e => e.Privata).HasColumnName("pl_privata");
            });

            modelBuilder.Entity<Artista>(b =>
            {
                b.ToTable("Artista");
                b.HasKey(e => e.Id);
                b.Property(e => e.Id).HasColumnName("ar_id");
                b.Property(e => e.Nome).HasColumnName("ar_nome").HasMaxLength(100);
            });
        }
    }
}