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
        public long AsalcCanzoneFk { get; set; }

        [Required]
        [Column("asalc_album_fk")]
        public long AsalcAlbumFk { get; set; }
    }
}