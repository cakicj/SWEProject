using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace GETOut.Models
{
    [Table("galerija")]
    public partial class Galerija
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("kafic_id")]
        public int KaficId { get; set; }

        [Column("url")]
        public string Url { get; set; }

    }
}
