using GETOut.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GETOut.Models
{
    public class SweContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Sto> Stolovi { get; set; }
        public DbSet<Kafic> Kafici { get; set; }
        public DbSet<ApplicationUser> Korisnici { get; set; }
        public DbSet<Galerija> Galerije { get; set; }
        public DbSet<Ocene> Ocene { get; set; }

        public SweContext (DbContextOptions<SweContext> options) :base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
