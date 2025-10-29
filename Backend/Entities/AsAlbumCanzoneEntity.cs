using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("as_album_canzone")]
    public class AsAlbumCanzoneEntity
    {
        [Key]
        [Column("asalc_id")]
        public long AsalcId { get; set; }

        [Required]
        [Column("asalc_canzone_fk")]
        [StringLength(255)]
        public string AsalcCanzoneFk { get; set; } = string.Empty;

        [Required]
        [Column("asalc_album_fk")]
        [StringLength(255)]
        public string AsalcAlbumFk { get; set; } = string.Empty;
    }
}