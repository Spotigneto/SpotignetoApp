using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("Utente")]
    public class ProfileEntity
    {
        [Key]
        [Column("ut_id")]
        [StringLength(255)]
        public string Id { get; set; } = string.Empty;

        [Required]
        [Column("ut_nome")]
        public string Nome { get; set; } = "";
    }
}
