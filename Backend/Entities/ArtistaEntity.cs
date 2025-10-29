using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("Artista")]
    public class ArtistaEntity
    {
        [Key]
        [Column("ar_id")]
        [StringLength(255)]
        public string ArId { get; set; } = string.Empty;

        [Required]
        [Column("ar_nome")]
        [StringLength(100)]
        public string ArNome { get; set; } = string.Empty;
    }
}