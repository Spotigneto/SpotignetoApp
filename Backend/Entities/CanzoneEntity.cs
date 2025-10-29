using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("Canzone")]
    public class CanzoneEntity
    {
        [Key]
        [Column("ca_id")]
        [StringLength(255)]
        public string CaId { get; set; } = string.Empty;

        [Required]
        [Column("ca_nome")]
        [StringLength(100)]
        public string CaNome { get; set; } = string.Empty;

        [Required]
        [Column("ca_file")]
        [StringLength(500)]
        public string? CaFile { get; set; }

        [Column("ca_genere")]
        [StringLength(100)]
        public string CaGenere { get; set; } = string.Empty;

        [Column("ca_sottogenere")]
        [StringLength(100)]
        public string CaSottogenere { get; set; } = string.Empty;

        [Column("ca_durata")]
        [StringLength(10)]
        public string CaDurata { get; set; } = string.Empty;

        [Column("ca_secondi")]
        public int CaSecondi { get; set; }
    }
}