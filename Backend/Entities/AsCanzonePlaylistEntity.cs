using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("as_canzone_playlist")]
    public class AsCanzonePlaylistEntity
    {
        [Key]
        [Column("ascp_id")]
        public long AscpId { get; set; }

        [Required]
        [Column("ascp_playlist_fk")]
        [StringLength(255)]
        public string AscpPlaylistFk { get; set; } = string.Empty;

        [Required]
        [Column("ascp_canzone_fk")]
        [StringLength(255)]
        public string AscpCanzoneFk { get; set; } = string.Empty;
    }
}