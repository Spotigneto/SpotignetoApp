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
        public long UtenteFk { get; set; }

        [Required]
        [Column("asup_playlist_fk")]
        public long PlaylistFk { get; set; }
    }
}