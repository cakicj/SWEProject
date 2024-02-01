using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GETOut.Models
{
    public class ApplicationUser : IdentityUser
    {

        [Column("Ime")]
        [Required(ErrorMessage = "Polje ime je obavezno")]
        public string Ime { get; set; }

        [Column("Prezime")]
        [Required(ErrorMessage = "Polje prezime je obavezno")]
        public string Prezime { get; set; }
    }
}
