using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("Sottogenere_tp")]
    public class SottogenereEntity
    {
        [Key]
        [Column("stp_id")]
        public long StpId { get; set; }

        [Required]
        [Column("stp_sottogenere")]
        [StringLength(100)]
        public string StpSottogenere { get; set; } = string.Empty;
    }
}
