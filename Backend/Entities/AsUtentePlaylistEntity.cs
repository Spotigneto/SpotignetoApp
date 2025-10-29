using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("as_utente_playlist")]
    public class AsUtentePlaylistEntity
    {
        [Key]
        [Column("asup_id")]
        public long Id { get; set; }

        [Required]
        [Column("asup_utente_fk")]
        [StringLength(255)]
        public string UtenteFk { get; set; } = string.Empty;

        [Required]
        [Column("asup_playlist_fk")]
        [StringLength(255)]
        public string PlaylistFk { get; set; } = string.Empty;
    }
}