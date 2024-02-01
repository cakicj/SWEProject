using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GETOut.Models;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace GETOut.Controllers
{
    public class GlavniController : ControllerBase
    {
        private readonly SweContext _context;

        public GlavniController(SweContext context)
        {
            _context = context;
        }

        #region Korisnik

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public Korisnik GetKorisnik(string id)
        {

          Korisnik kor = new Korisnik();
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);

          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
          cmd.CommandText = "SELECT * FROM aspnetusers WHERE `id` = '" + id + "'";
          MySqlDataReader reader = cmd.ExecuteReader();
          while (reader.Read())
          {
                    kor.Id = reader["Id"].ToString();
                    kor.Ime = reader["Ime"].ToString();
                    kor.Prezime = reader["Prezime"].ToString();
                    kor.Username = reader["UserName"].ToString();
                    kor.BrTelefona = reader["PhoneNumber"].ToString();
                    kor.Email = reader["Email"].ToString();
          }
          reader.Close();
          cmd.Dispose();
          conn.Close();

          return kor;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("ObrisiKorisnika/{id}")]
        public ActionResult DeleteUser(string id)
        {
          Korisnik kor = GetKorisnik(id);
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          PronadjiSto(kor.Username);
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
            try
            {
                cmd.CommandText = "DELETE FROM `aspnetusers` WHERE `Id`= '" + id + "'";
                cmd.ExecuteNonQuery();
            }
          
            catch (Exception)
            {
                cmd.Dispose();
                conn.Close();
                return BadRequest("Nije moguce obrisati korisnika!");
            }

            cmd.Dispose();
            conn.Close();
            return Ok();
        }

        #endregion

        
        #region Kafic

        [AllowAnonymous]
        [HttpGet]
        [Route("VratiKafice")]
        public List<Kafic> GetCafes()
        {
          List<Kafic> kafici = new List<Kafic>();
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);

          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
          cmd.CommandText = "SELECT * FROM kafic";
          MySqlDataReader reader = cmd.ExecuteReader();
          while (reader.Read())
          {
            kafici.Add(new Kafic
            {
              Id = Convert.ToInt32(reader["id"]),
              Naziv = reader["naziv"].ToString(),
              Ocena = (float)reader["ocena"],
              Adresa = reader["adresa"].ToString(),
              Telefon = reader["telefon"].ToString(),
              Dogadjaj = reader["dogadjaj"].ToString(),              
              UkupanKapacitet = Convert.ToInt32(reader["ukupan_kapacitet"]),
              RadnoVreme = reader["radno_vreme"].ToString(),
              MapaX = (float)reader["mapaX"],
              MapaY = (float)reader["mapaY"],
              Stos = VratiStolove(Convert.ToInt32(reader["id"])),
              Galerijas = VratiGalerije(Convert.ToInt32(reader["id"]))
            });

          }

          reader.Close();
          cmd.Dispose();
          conn.Close();

          return kafici;
        }

        // vraca odredjeni kafic po id-u

        [AllowAnonymous]
        [HttpGet]
        [Route("VratiKafic/{id}")]
        public Kafic GetCafeById(int id)
        {
          Kafic kafic = new Kafic();
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);

          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
          cmd.CommandText = "SELECT * FROM kafic WHERE id = '" + id + "'";
          MySqlDataReader reader = cmd.ExecuteReader();
          if (reader.Read())
          {
            kafic.Id = Convert.ToInt32(reader["id"]);
            kafic.Naziv = reader["naziv"].ToString();
            kafic.Ocena = (float)reader["ocena"];
            kafic.Adresa = reader["adresa"].ToString();
            kafic.Telefon = reader["telefon"].ToString();
            kafic.Dogadjaj = reader["dogadjaj"].ToString();
            kafic.UkupanKapacitet = Convert.ToInt32(reader["ukupan_kapacitet"]);
            kafic.RadnoVreme = reader["radno_vreme"].ToString();
            kafic.MapaX = (float)reader["mapaX"];
            kafic.MapaY = (float)reader["mapaY"];
            kafic.Stos = VratiStolove(id);
            kafic.Galerijas = VratiGalerije(id);
          }


          reader.Close();
          cmd.Dispose();
          conn.Close();

          return kafic;
        }

        // dodaje kafic

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,Moderatori")]
        [HttpPost]
        [Route("DodajKafic")]
        public async Task<ActionResult<Kafic>> PostKafic([FromBody] Kafic kafic)
        {
          {
            if (!ModelState.IsValid)
                return BadRequest("Broj telefona nije validan!");

            Kafic kafic2 = new Kafic
            {
              Naziv = kafic.Naziv,
              Telefon = kafic.Telefon,
              Ocena = 0,
              Adresa = kafic.Adresa,
              RadnoVreme = kafic.RadnoVreme,
              UkupanKapacitet = 0,
              MapaX = 0,
              MapaY = 0
            };
                try
                {
                    _context.Kafici.Add(kafic2);
                    await _context.SaveChangesAsync();
                }
            
                catch (Exception)
                {
                    return BadRequest("Niste uneli validne podatke!");
                }

            return CreatedAtAction("getCafeById", new { id = kafic2.Id }, kafic2);
          }
        }

        // uredjivanje kafica

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,Moderatori")]
        [HttpPut]
        [Route("IzmeniKafic/{id}")]
        public ActionResult<Kafic> EditCafe([FromBody] Kafic kaf, int id)
        {
            if (kaf == null)
                return BadRequest("Niste uneli validne koordinate!");

            String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
            try
            {  
                cmd.CommandText = "UPDATE `kafic` SET `mapaX`=@MapaX, `mapaY`=@MapaY, `naziv`=@Naziv, `adresa`= @Adresa, `dogadjaj`= @Dogadjaj, `ukupan_kapacitet`= @Ukupan_kapacitet, `radno_vreme`= @RadnoVreme WHERE `id`= '" + id + "'";
                cmd.Parameters.AddWithValue("@Naziv", kaf.Naziv);
                cmd.Parameters.AddWithValue("@Adresa", kaf.Adresa);
                cmd.Parameters.AddWithValue("@Dogadjaj", kaf.Dogadjaj);
                cmd.Parameters.AddWithValue("@Ukupan_kapacitet", GetCapacity(id));
                cmd.Parameters.AddWithValue("@RadnoVreme", kaf.RadnoVreme);
                cmd.Parameters.AddWithValue("@MapaX", kaf.MapaX);
                cmd.Parameters.AddWithValue("@MapaY", kaf.MapaY);
                cmd.ExecuteNonQuery();
            }

            catch(Exception)
            {
                cmd.Dispose();
                conn.Close();
                return BadRequest("Niste uneli validne podatke!");
            }

          cmd.Dispose();
          conn.Close();
          return GetCafeById(id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,Moderatori")]
        [HttpPut]
        [Route("IzmeniDogadjajKafica/{id}/{dogadjaj}")]
        public ActionResult<Kafic> EditDogadjajKafica(int id, String dogadjaj)
        {
            String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            try
            {
                cmd.CommandText = "UPDATE `kafic` SET `dogadjaj`= @Dogadjaj WHERE `id`= '" + id + "'";
                cmd.Parameters.AddWithValue("@Dogadjaj", dogadjaj);
                cmd.ExecuteNonQuery();
            }
            
            catch (Exception)
            {
                cmd.Dispose();
                conn.Close();
                return BadRequest("Podatak nije validan");
            }

            cmd.Dispose();
            conn.Close();
            return GetCafeById(id);

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,Moderatori")]
        [HttpDelete]
        [Route("ObrisiKafic/{id}")]
        public ActionResult DeleteCafe(int id)
        {
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
            try
            {
                cmd.CommandText = "DELETE FROM `kafic` WHERE `id`=' " + id + "'";
                cmd.ExecuteNonQuery();
            }
         
            catch (Exception)
            {
                cmd.Dispose();
                conn.Close();
                return BadRequest("Doslo je do greske!");
            }

          cmd.Dispose();
          conn.Close();
          return Ok();
        }
        
        #region Metode za prosecnu ocenu

        [HttpGet]
        [Route("VratiProsecnuOcenu/{id}")]
        public float RatingOfCafe(int id)
        {
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          conn.Open();
          Kafic kaf = new Kafic();
          MySqlCommand cmd = conn.CreateCommand();
          cmd.CommandText = "SELECT kafic.ocena FROM swe.kafic WHERE id = '" + id + "'";
          MySqlDataReader reader = cmd.ExecuteReader();
          while (reader.Read())
          {
            kaf.Ocena = (float)reader["ocena"];
          }

          reader.Close();
          cmd.Dispose();
          conn.Close();
          return (float)kaf.Ocena;
        }

        [HttpGet]
        [Route("VratiBrOcena/{id}")]
        public int CountId(int id)
        {
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          int count = 0;
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
          cmd.CommandText = "SELECT COUNT(*) FROM swe.ocene WHERE ocene.kafic_id = '" + id + "'";
          MySqlDataReader reader = cmd.ExecuteReader();
          while (reader.Read())
          {
            count = Convert.ToInt32(reader["COUNT(*)"]);
          }

          cmd.Dispose();
          conn.Close();
          return count;

        }


        [HttpPut]
        [Route("Ocene/{id}")]
        public void RateCafe([FromBody] int id, int taOcena)
        {
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          float pOcena = RatingOfCafe(id);
          int brPonovavljanja = CountId(id);
          float novaProsecnaOcena = (pOcena * (brPonovavljanja - 1) + taOcena) / brPonovavljanja; // ( prosecnaOcena * brojOcena + novaOcena ) / (brojOcena + 1)
          float prikaz = (float)Math.Round(novaProsecnaOcena * 100f) / 100f;
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
          cmd.CommandText = "UPDATE `kafic` SET `ocena` = @ProsecnaOcena WHERE `id`= '" + id + "'";
          cmd.Parameters.AddWithValue("@ProsecnaOcena", prikaz);
          cmd.ExecuteNonQuery();

          cmd.Dispose();
          conn.Close();
        }

        [HttpGet]
        [Route("ProveriDaLiJeVecDaoOcenu/{idKafica}/{idKorisnika}")]
        public int CheckMark(int idKafica, string idKorisnika)
        {
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          int proveri = 0;
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
          cmd.CommandText = "SELECT COUNT(*) FROM swe.ocene WHERE `kafic_id` = '" + idKafica + "'" + " AND `korisnik_id` = '" + idKorisnika + "'";
          MySqlDataReader reader = cmd.ExecuteReader();
          while (reader.Read())
          {
            proveri = Convert.ToInt32(reader["COUNT(*)"]);
          }

          return proveri;
        }

        #endregion

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("DodajOcenu/{idKafic}/{idKorisnik}/{ocena}")]
        public ActionResult<Kafic> AddRate(int idKafic, string idKorisnik, int ocena)
        {
            int provera = CheckMark(idKafic, idKorisnik);
            if (provera == 0)
            {
                String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO `ocene` (`kafic_id`, `korisnik_id`, `ocena`) VALUES (@Kafic, @Korisnik, @Ocena)";
                cmd.Parameters.AddWithValue("@Kafic", idKafic);
                cmd.Parameters.AddWithValue("@Korisnik", idKorisnik);
                cmd.Parameters.AddWithValue("@Ocena", ocena);
                cmd.ExecuteNonQuery();
                RateCafe(idKafic, ocena);

                cmd.Dispose();
                conn.Close();

                return Ok(new { Kafic = GetCafeById(idKafic) });
            }
            return BadRequest("Vec ste ocenili ovaj kafic!");
        }

        #endregion


        #region Sto

        [AllowAnonymous]
        [HttpGet]
        [Route("VratiStolove")]
        public async Task<ActionResult<IEnumerable<Sto>>> GetStos()
        {
          return await _context.Stolovi.ToListAsync();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("VratiSto/{id}")]
        public async Task<ActionResult<Sto>> GetSto(int id)
        {
          var sto = await _context.Stolovi.FindAsync(id);

          if (sto == null)
          {
            return NotFound();
          }

          return sto;
        }

        #region Metode za kapacitet

        [AllowAnonymous]
        [HttpGet]
        [Route("VratiBrOsoba/{id}")]
        public int GetCapacity(int id)
        {
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          List<Sto> sto = new List<Sto>();
          MySqlConnection conn = new MySqlConnection(connectionString);
          conn.Open();
          int count = 0;
          MySqlCommand cmd = conn.CreateCommand();
          cmd.CommandText = "SELECT broj_osoba FROM swe.sto WHERE `kafic_id`= '" + id + "'";
          MySqlDataReader reader = cmd.ExecuteReader();
          while (reader.Read())
          {
            sto.Add(new Sto
            {
              BrojOsoba = Convert.ToInt32(reader["broj_osoba"])
            }); 
          }
          foreach (Sto s in sto)
          {
            count += s.BrojOsoba;
          }
          cmd.Dispose();
          conn.Close();
          return count;
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("IzmeniKapacitetKafica/{id}")]
        public void EditCapacity([FromBody] int id)
        {
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);

          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
          cmd.CommandText = "UPDATE `kafic` SET `ukupan_kapacitet`= @Ukupan_kapacitet WHERE `id`= '" + id + "'";
          cmd.Parameters.AddWithValue("@Ukupan_kapacitet", GetCapacity(id));
          cmd.ExecuteNonQuery();

          cmd.Dispose();
          conn.Close();
        }
        #endregion

        [AllowAnonymous]
        [HttpGet]
        [Route("NekaTamo")]
        public Sto GetSpecificTable(int idKafica, int brOsoba, string idKorisnika)
        {
          Sto sto2 = new Sto();
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
          cmd.CommandText = "SELECT * FROM `sto` WHERE `rezervisan` = 'true' AND `kafic_id`= '" + idKafica + "'" + "AND `broj_osoba`= '" + brOsoba + "'" + " LIMIT 1";
          MySqlDataReader reader = cmd.ExecuteReader();
          while (reader.Read())
          {
            Sto sto1 = new Sto
            {
              Id = Convert.ToInt32(reader["id"]),
              KaficId = Convert.ToInt32(reader["kafic_id"]),
              Username = reader["username"].ToString(),
              BrojOsoba = Convert.ToInt32(reader["broj_osoba"]),
              Rezervisan = reader["rezervisan"].ToString()
            };
            sto2 = sto1;
          }

          reader.Close();
          cmd.Dispose();
          conn.Close();

          return sto2;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [Route("DodajRezervaciju/{idKorisnika}/{brOsoba}/{idKafica}")]
        public ActionResult<Sto> PutSto(string idKorisnika, int brOsoba, int idKafica)
        {
            String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
            MySqlConnection conn = new MySqlConnection(connectionString);
            Korisnik kor = GetKorisnik(idKorisnika);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            try
            {
                cmd.CommandText = "UPDATE `sto` SET `username`= @Username, `rezervisan` = @Rezervisan, `username` = @Username WHERE `kafic_id`= '" + idKafica + "'" + " AND `broj_osoba`= '" + brOsoba + "'" + " AND `rezervisan` = 'false' LIMIT 1";
                cmd.Parameters.AddWithValue("@Username", kor.Username);
                cmd.Parameters.AddWithValue("@Rezervisan", "true");
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                cmd.Dispose();
                conn.Close();
                return BadRequest("Nije moguce rezervisati");
            }

            cmd.Dispose();
            conn.Close();
            return GetSpecificTable(idKafica, brOsoba, idKorisnika);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [Route("UkloniRezervaciju/{id}")]
        public async Task<ActionResult<Sto>> DeleteRezervacija(int id)
        {
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
          try
          {
            cmd.CommandText = "UPDATE `sto` SET `rezervisan` = @Rezervisan, `username` = @Username WHERE `id`= '" + id + "'";
            cmd.Parameters.AddWithValue("@Username", null);
            cmd.Parameters.AddWithValue("@Rezervisan", "false");
            cmd.ExecuteNonQuery();
          }

          catch (Exception)
          {
                cmd.Dispose();
                conn.Close();
                return BadRequest("Nije moguce ukloniti rezervaciju"); 
          }

          cmd.Dispose();
          conn.Close();

          return await GetSto(id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,Moderatori")]
        [HttpPost]
        [Route("DodajSto")]
        public async Task<ActionResult<Sto>> PostTable([FromBody] Sto sto)
        {
          {
            Sto sto2 = new Sto
            {
              KaficId = sto.KaficId,
              BrojOsoba = sto.BrojOsoba,
              Rezervisan = "false"
            };
                try
                {
                    _context.Stolovi.Add(sto2);
                    await _context.SaveChangesAsync();
                }

                catch (Exception)
                {
                    return BadRequest("Nije moguce dodati sto!");
                }
            
            GetCapacity(sto.KaficId);
            EditCapacity(sto.KaficId);

            return CreatedAtAction("getSto", new { id = sto2.Id }, sto2);
          }
        }

        // vraca listu stolova u kaficu

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public List<Sto> VratiStolove(int id)
        {
          List<Sto> stolovi = new List<Sto>();
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
          cmd.CommandText = "SELECT * FROM swe.sto WHERE `kafic_id` = '" + id + "'";
          MySqlDataReader reader = cmd.ExecuteReader();
          while (reader.Read())
          {
            stolovi.Add(new Sto
            {
              Id = Convert.ToInt32(reader["id"]),
              KaficId = Convert.ToInt32(reader["kafic_id"]),
              Username = reader["username"].ToString(),
              BrojOsoba = Convert.ToInt32(reader["broj_osoba"]),
              Rezervisan = reader["rezervisan"].ToString()
            });
                
          }
          reader.Close();
          cmd.Dispose();
          conn.Close();

          return stolovi;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public void PronadjiSto(string username)
        {
            String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE `sto` SET `rezervisan` = 'false' WHERE `username` = '" + username + "'";
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Close();
        }

        [HttpGet]
        public Sto ZaRacunanje(int id)
        {
          Sto sto = new Sto();
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
          cmd.CommandText = "SELECT * FROM swe.sto WHERE `id` = '" + id + "'";
          MySqlDataReader reader = cmd.ExecuteReader();
          while (reader.Read())
          {
            sto.Id = id;
            sto.KaficId = Convert.ToInt32(reader["kafic_id"]);
          }

          reader.Close();
          cmd.Dispose();
          conn.Close();

          return sto;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,Moderatori")]
        [HttpDelete]
        [Route("ObrisiSto/{id}")]
        public ActionResult DeleteTable(int id)
        {
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          Sto sto = ZaRacunanje(id);
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
            try
            {
                cmd.CommandText = "DELETE FROM `sto` WHERE `id`=' " + id + "'";
                cmd.ExecuteNonQuery();
            }

            catch (Exception)
            {
                cmd.Dispose();
                conn.Close();
                return BadRequest("Nije moguce obrisati sto!");
            }
        
            cmd.Dispose();
            conn.Close();
           
            GetCapacity(sto.KaficId);
            EditCapacity(sto.KaficId);

           return Ok();
        }

        #endregion


        #region Galerija

        [AllowAnonymous]
        [HttpGet]
        [Route("VratiGalerije")]
        public async Task<ActionResult<IEnumerable<Galerija>>> GetGalerije()
        {
          return await _context.Galerije.ToListAsync();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("VratiGaleriju/{id}")]
        public async Task<ActionResult<Galerija>> GetGalerija(int id)
        {
          var galerija = await _context.Galerije.FindAsync(id);

          if (galerija == null)
          {
            return NotFound();
          }

          return galerija;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,Moderatori")]
        [HttpPost]
        [Route("DodajGaleriju")]
        public async Task<ActionResult<Galerija>> PostGalerija([FromBody] Galerija galerija)
        {
          {
            Galerija galerija2 = new Galerija
            {
              KaficId = galerija.KaficId,
              Url = galerija.Url
            };
                try
                {
                    _context.Galerije.Add(galerija2);
                    await _context.SaveChangesAsync();
                }

                catch (Exception)
                {
                    return BadRequest("Doslo je do greske!");
                }
           

            return CreatedAtAction("GetGalerija", new { id = galerija2.Id }, galerija2);
          }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,Moderatori")]
        [HttpDelete]
        [Route("ObrisiGaleriju/{id}")]
        public ActionResult DeleteGalerija(int id)
        {
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
            try
            {
                cmd.CommandText = "DELETE FROM `galerija` WHERE `id`=' " + id + "'";
                cmd.ExecuteNonQuery();
            }
          
            catch (Exception)
            {
                cmd.Dispose();
                conn.Close();
                return BadRequest("Doslo je do greske!");
            }

            cmd.Dispose();
            conn.Close();
            return Ok();
        }

        [HttpGet]
        [Route("FunkcijaZaGalerije")]
        public List<Galerija> VratiGalerije(int id)
        {
          List<Galerija> galerija = new List<Galerija>();
          String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
          MySqlConnection conn = new MySqlConnection(connectionString);
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand();
          cmd.CommandText = "SELECT * FROM swe.galerija WHERE `kafic_id` = '" + id + "'";
          MySqlDataReader reader = cmd.ExecuteReader();
          while (reader.Read())
          {
            galerija.Add(new Galerija
            {
              Id = Convert.ToInt32(reader["id"]),
              KaficId = Convert.ToInt32(reader["kafic_id"]),
              Url = reader["url"].ToString()
            });
          }
          reader.Close();
          cmd.Dispose();
          conn.Close();

          return galerija;
        }

        #endregion

    }
}
