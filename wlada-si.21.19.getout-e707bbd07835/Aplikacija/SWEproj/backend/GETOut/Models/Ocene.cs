using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace GETOut.Models
{
  [Table("ocene")]
  public partial class Ocene
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("kafic_id")]
        public int KaficId { get; set; }

        [Column("korisnik_id")]
        public string KorisnikId { get; set; }

        [Column("ocena")]
        public int Ocena { get; set; }

    }
}
