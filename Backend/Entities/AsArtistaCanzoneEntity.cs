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
        public long ArtistaFk { get; set; }

        [Required]
        [Column("asarc_canzone_fk")]
        public long CanzoneFk { get; set; }
    }
}