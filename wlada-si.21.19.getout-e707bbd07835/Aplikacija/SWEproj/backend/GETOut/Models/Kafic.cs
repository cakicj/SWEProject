using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace GETOut.Models
{
  [Table("kafic")]
  public class Kafic
  {
    public Kafic()
    {
        Galerijas = new HashSet<Galerija>();
        Stos = new HashSet<Sto>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("naziv")]
    public string Naziv { get; set; }

    [Column("ocena")]
    public float? Ocena { get; set; }

    [Column("adresa")]
    public string Adresa { get; set; }

    [Column("telefon")]
    [StringLength(13, MinimumLength = 6)]
    [Phone]
    public string Telefon { get; set; }

    [Column("dogadjaj")]
    public string Dogadjaj { get; set; }

    [Column("ukupan_kapacitet")]
    public int? UkupanKapacitet { get; set; }

    [Column("radno_vreme")]
    public string RadnoVreme { get; set; }

    [Column("mapaX")]
    public float MapaX { get; set; }

    [Column("mapaY")]
    public float MapaY { get; set; }

    public ICollection<Galerija> Galerijas { get; set; }
    public ICollection<Sto> Stos { get; set; }
        
  }
}
