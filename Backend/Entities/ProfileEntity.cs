using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("Utente")]
    public class ProfileEntity
    {
        [Key]
        [Column("ut_id")]
        public long UtId { get; set; }

        [Required]
        [Column("ut_nome")]
        [StringLength(100)]
        public string UtNome { get; set; } = string.Empty;
    }
}
