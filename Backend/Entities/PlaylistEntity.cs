using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("Playlist")]
    public class PlaylistEntity
    {
        [Key]
        [Column("pl_id")]
        [StringLength(255)]
        public string PlId { get; set; } = string.Empty;

        [Required]
        [Column("pl_nome")]
        [StringLength(100)]
        public string PlNome { get; set; } = string.Empty;

        [Column("pl_privata")]
        public bool PlPrivata { get; set; }
    }
}