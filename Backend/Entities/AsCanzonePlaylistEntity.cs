using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("AsCanzonePlaylist")]
    public class AsCanzonePlaylistEntity
    {
        [Key]
        [Column("ac_id")]
        public long AcId { get; set; }

        [Required]
        [Column("ac_playlist_id")]
        public long AcPlaylistId { get; set; }

        [Required]
        [Column("ac_canzone_id")]
        public long AcCanzoneId { get; set; }

        [Column("ac_track_order")]
        public int? AcTrackOrder { get; set; }
    }
}