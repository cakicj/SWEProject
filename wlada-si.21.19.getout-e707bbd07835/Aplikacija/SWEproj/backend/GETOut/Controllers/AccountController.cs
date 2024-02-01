using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GETOut.Authentication;
using GETOut.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;

namespace GETOut.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        // registracija

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Registration([FromBody] Register reg)
        {

            if (ModelState.IsValid)
            {
                var userExist = await userManager.FindByNameAsync(reg.UserName);
                if (userExist != null)
                    return BadRequest("Ovaj username je vec u upotrebi!");

                var userEmail = await userManager.FindByEmailAsync(reg.Email);
                if (userEmail != null)
                    return BadRequest("Ovaj email je vec u upotrebi!");

                var applicationUser = new ApplicationUser()
                {
                    Ime = reg.Ime,
                    Email = reg.Email,
                    Prezime = reg.Prezime,
                    UserName = reg.UserName,
                    PhoneNumber = reg.BrTelefona
                };
                if (!await roleManager.RoleExistsAsync(UserRole.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
                }

                if (!await roleManager.RoleExistsAsync(UserRole.Moderatori))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRole.Moderatori));
                }
                if (!await roleManager.RoleExistsAsync(UserRole.Prijavljeni))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRole.Prijavljeni));
                }

                try
                {
                    var result = await userManager.CreateAsync(applicationUser, reg.Password);
                    await userManager.AddToRoleAsync(applicationUser, UserRole.Prijavljeni);
                    return Ok(result);
                }

                catch (Exception)
                {
                    return BadRequest("Sifra mora da sadrzi najmanje 6 karaktera, da sadrzi jedno veliko slovo, jedan broj i jedan specijalni znak!");
                }
            }
            else
                return BadRequest("Podaci nisu validni!");
        }

        // logovanje

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Login log)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(log.UserName);
                if (user != null && await userManager.CheckPasswordAsync(user, log.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    var uloga = userRoles.FirstOrDefault();
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var authSiginKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddDays(1),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSiginKey, SecurityAlgorithms.HmacSha256Signature)
                        );

                    return Ok(new Korisnik
                    {
                        Uloga = uloga,
                        Id = user.Id,
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        ValidTo = token.ValidTo.ToString("yyyy-MM-ddThh:mm:ss"),
                        Username = user.UserName,
                        Password = log.Password,
                        Ime = user.Ime,
                        Prezime = user.Prezime,
                        Email = user.Email,
                        BrTelefona = user.PhoneNumber                       
                    });
                }
            }
            return BadRequest("Pogresan username ili password");
        }

        // Unapredjivanje u moderatora

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [Route("Unapredi/{userName}")]
        public async Task <IActionResult> UpdateRole(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user != null)
            {
                if (await userManager.IsInRoleAsync(user, UserRole.Prijavljeni) == true)
                {
                    await userManager.RemoveFromRoleAsync(user, UserRole.Prijavljeni);
                    await userManager.AddToRoleAsync(user, UserRole.Moderatori);
                    return Ok("Uspesno ste unapredili!");
                }
            }
            return BadRequest("Doslo je do greske!");
        }

        #region Korisnik

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("VratiKorisnika/{id}")]
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

        [AllowAnonymous]
        [HttpGet]
        [Route("ProveriKorisnike")]
        public List<Korisnik> ProveriKorisnike()
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT aspnetusers.Id, aspnetusers.UserName, aspnetusers.Email FROM swe.aspnetusers";
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                korisnici.Add(new Korisnik
                {
                    Id = reader["Id"].ToString(),
                    Username = reader["UserName"].ToString(),
                    Email = reader["Email"].ToString(),
                }); ;
            }
            reader.Close();
            cmd.Dispose();
            conn.Close();

            return korisnici;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Prijavljeni")]
        [HttpPut]
        [Route("IzmeniKorisnika/{id}")]
        public async Task<ActionResult<Korisnik>> EditUser([FromBody] Korisnik kor, string id)
        {
            var applicationUser = await userManager.FindByIdAsync(id);
            if (await userManager.IsInRoleAsync(applicationUser, UserRole.Prijavljeni) == false)
                return BadRequest("Doslo je do greske!");

            if (applicationUser.Ime == kor.Ime && applicationUser.Email == kor.Email && applicationUser.Prezime == kor.Prezime && applicationUser.UserName == kor.Username && applicationUser.PhoneNumber == kor.BrTelefona)
                return BadRequest("Niste izmenili podatke!");

            applicationUser.Ime = kor.Ime;
            applicationUser.Email = kor.Email;
            applicationUser.Prezime = kor.Prezime;
            applicationUser.UserName = kor.Username;
            applicationUser.PhoneNumber = kor.BrTelefona;

            var token = await userManager.GeneratePasswordResetTokenAsync(applicationUser);

            if (ModelState.IsValid)
            {
                List<Korisnik> korisniks = ProveriKorisnike();
                kor.Id = id;
                kor.Uloga = "Prijavljeni";
                bool i = false;
                foreach (Korisnik k in korisniks)
                {
                    if (k.Id != kor.Id)
                    {
                        if (k.Username == kor.Username)
                        {
                            i = true;
                            return BadRequest("Ovaj username je vec u upotrebi!");
                        }
                        if (k.Email == kor.Email)
                        {
                            i = true;
                            return BadRequest("Ovaj email je vec u upotrebi!");
                        }
                    }

                }
                if (i == false)
                {
                    var result = await userManager.ResetPasswordAsync(applicationUser, token, kor.Password);
                    if (!result.Succeeded)
                        return BadRequest("Sifra mora da sadrzi najmanje 6 karaktera, da sadrzi jedno veliko slovo, jedan broj i jedan specijalni znak!");

                    await userManager.UpdateAsync(applicationUser);
                }
            }
            else
                return BadRequest("Podaci nisu validni");
           
            return CreatedAtAction("GetKorisnik", new { id = kor.Id }, kor);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("VratiKorisnike")]
        public async Task<List<Korisnik>> GetKorisnikeAsync()
        {

            List<Korisnik> korisnici = new List<Korisnik>();
            String connectionString = "server = localhost;user id = root;database = swe;password = root; Persist Security Info=False";
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM aspnetusers";
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                korisnici.Add(new Korisnik
                {
                    Id = reader["Id"].ToString(),
                    Ime = reader["Ime"].ToString(),
                    Prezime = reader["Prezime"].ToString(),
                    Username = reader["UserName"].ToString(),
                    BrTelefona = reader["PhoneNumber"].ToString(),
                    Email = reader["Email"].ToString()
                });

            }
            foreach (Korisnik kor in korisnici)
            {
                var appUser = await userManager.FindByIdAsync(kor.Id);
                var userRoles = await userManager.GetRolesAsync(appUser);
                var uloga = userRoles.FirstOrDefault();
                kor.Uloga = uloga;
            }

            reader.Close();
            cmd.Dispose();
            conn.Close();

            return korisnici;
        }

        #endregion
    }
}
