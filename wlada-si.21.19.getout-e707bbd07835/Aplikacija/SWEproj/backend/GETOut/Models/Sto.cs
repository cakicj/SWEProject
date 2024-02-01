using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace GETOut.Models
{
    [Table("sto")]
    public partial class Sto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("kafic_id")]
        public int KaficId { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("broj_osoba")]
        public int BrojOsoba { get; set; }

        [Column("rezervisan")]
        public string Rezervisan { get; set; }

    }
}
