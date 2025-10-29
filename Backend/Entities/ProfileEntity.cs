using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("Utente")]
    public class ProfileEntity
    {
        [Key]
        [Column("ut_id")]
        public long Id { get; set; }

        [Required]
        [Column("ut_nome")]
        public string Nome { get; set; } = "";
    }
}
