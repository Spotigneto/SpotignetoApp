using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("as_artista_album")]
    public class AsArtistaAlbumEntity
    {
        [Key]
        [Column("asaa_id")]
        public long Id { get; set; }

        [Required]
        [Column("asaa_artista_fk")]
        [StringLength(255)]
        public string ArtistaFk { get; set; } = string.Empty;

        [Required]
        [Column("asaa_album_fk")]
        [StringLength(255)]
        public string AlbumFk { get; set; } = string.Empty;
    }
}