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
        [StringLength(255)]
        public string AlNome { get; set; } = string.Empty;

        [Column("al_anno")]
        public int AlAnno { get; set; }

        [Column("al_copertina")]
        [StringLength(500)]
        public string? AlCopertina { get; set; }
    }
}