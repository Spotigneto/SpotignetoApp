using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("Canzone")]
    public class CanzoneEntity
    {
        [Key]
        [Column("ca_id")]
        public long CaId { get; set; }

        [Required]
        [Column("ca_nome")]
        [StringLength(100)]
        public string CaNome { get; set; } = string.Empty;

        [Required]
        [Column("ca_file")]
        [StringLength(500)]
        public string CaFile { get; set; } = string.Empty;

        [Column("ca_genere_fk")]
        public long CaGenere { get; set; }

        [Column("ca_sottogenere_fk")]
        public long CaSottogenere { get; set; }

        [Column("ca_durata")]
        [StringLength(10)]
        public string CaDurata { get; set; } = string.Empty;

        [Column("ca_secondi")]
        public int CaSecondi { get; set; }
    }
}