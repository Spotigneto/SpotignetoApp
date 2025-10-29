using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("Album")]
    public class AlbumEntity
    {
        [Key]
        [Column("al_id")]
        public long AlId { get; set; }

        [Required]
        [Column("al_nome")]
        [StringLength(100)]
        public string AlNome { get; set; } = string.Empty;

        [Column("al_pubblica")]
        public bool AlPubblica { get; set; }

        [Column("al_release_date")]
        public DateTime? AlReleaseDate { get; set; }
    }
}