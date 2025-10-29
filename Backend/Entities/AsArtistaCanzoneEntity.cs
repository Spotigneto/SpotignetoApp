using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("as_artista_canzone")]
    public class AsArtistaCanzoneEntity
    {
        [Key]
        [Column("asarc_id")]
        public long Id { get; set; }

        [Required]
        [Column("asarc_artista_fk")]
        [StringLength(255)]
        public string ArtistaFk { get; set; } = string.Empty;

        [Required]
        [Column("asarc_canzone_fk")]
        [StringLength(255)]
        public string CanzoneFk { get; set; } = string.Empty;
    }
}