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
        public DbSet<Backend.Services.Canzone> Canzone { get; set; }
        public DbSet<Backend.Services.Playlist> Playlist { get; set; }
        public DbSet<Backend.Services.Artista> Artista { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure your entities here
            // Example:
            // modelBuilder.Entity<Track>()
            //     .HasKey(t => t.Id);

            // Map the simplified shapes onto the existing tables/columns
            modelBuilder.Entity<Backend.Services.Canzone>(b =>
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

            modelBuilder.Entity<Backend.Services.Playlist>(b =>
            {
                b.ToTable("Playlist");
                b.HasKey(e => e.Id);
                b.Property(e => e.Id).HasColumnName("pl_id");
                b.Property(e => e.Nome).HasColumnName("pl_nome").HasMaxLength(100);
                b.Property(e => e.Privata).HasColumnName("pl_privata");
            });

            modelBuilder.Entity<Backend.Services.Artista>(b =>
            {
                b.ToTable("Artista");
                b.HasKey(e => e.Id);
                b.Property(e => e.Id).HasColumnName("ar_id");
                b.Property(e => e.Nome).HasColumnName("ar_nome").HasMaxLength(100);
            });
        }
    }
}

// Lightweight simplified entity shapes used by services/controllers.
// Kept in this file per request to only modify SpotigneteDbContext.cs.
namespace Backend.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Canzone")]
    public class Canzone
    {
        [Key]
        [Column("ca_id")]
        public long Id { get; set; }

        [Column("ca_nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("ca_file")]
        public string File { get; set; } = string.Empty;

        [Column("ca_genere_fk")]
        public long GenereFk { get; set; }

        [Column("ca_sottogenere_fk")]
        public long SottogenereFk { get; set; }

        [Column("ca_durata")]
        public string Durata { get; set; } = string.Empty;

        [Column("ca_secondi")]
        public int Secondi { get; set; }
    }

    [Table("Playlist")]
    public class Playlist
    {
        [Key]
        [Column("pl_id")]
        public long Id { get; set; }

        [Column("pl_nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("pl_privata")]
        public bool Privata { get; set; }
    }

    [Table("Artista")]
    public class Artista
    {
        [Key]
        [Column("ar_id")]
        public long Id { get; set; }

        [Column("ar_nome")]
        public string Nome { get; set; } = string.Empty;
    }
}