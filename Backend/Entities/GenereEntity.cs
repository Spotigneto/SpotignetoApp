using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entities
{
    [Table("Genere_tp")]
    public class GenereEntity
    {
        [Key]
        [Column("gtp_id")]
        public long GtpId { get; set; }

        [Required]
        [Column("gtp_genere")]
        [StringLength(100)]
        public string GtpGenere { get; set; } = string.Empty;
    }
}
