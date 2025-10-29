using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("Artista")]
    public class ArtistaEntity
    {
        [Key]
        [Column("ar_id")]
        public long ArId { get; set; }

        [Required]
        [Column("ar_nome")]
        [StringLength(100)]
        public string ArNome { get; set; } = string.Empty;
    }
}