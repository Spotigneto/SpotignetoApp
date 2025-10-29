using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("as_utente_artista")]
    public class AsUtenteArtistaEntity
    {
        [Key]
        [Column("asua_id")]
        public long AsuaId { get; set; }

        [Required]
        [Column("asua_utente_fk")]
        [StringLength(255)]
        public string AsuaUtenteFk { get; set; } = string.Empty;

        [Required]
        [Column("asua_artista_fk")]
        [StringLength(255)]
        public string AsuaArtistaFk { get; set; } = string.Empty;
    }
}