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

        [Column("ca_genere")]
        public long CaGenere { get; set; }

        [Column("ca_sottogenere")]
        public long CaSottogenere { get; set; }

        [Column("ca_durata")]
        public int CaDurata { get; set; }
    }
}