using KlasePodataka;
using KorisnickiInterfejsMVC.Models;
using PoslovnaLogika;
using Repozitorijumi;
using Servisi;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace KorisnickiInterfejsMVC.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student/Prijava
        [HttpGet]
        public ActionResult Prijava()
        {
            StudentPrijavljivanjeVM model = new StudentPrijavljivanjeVM();

            return View(model);
        }

        // POST: Student/Prijava
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Prijava(
     StudentPrijavljivanjeVM model)
        {
            byte tipKorisnika;

            if (Session["KorisnikID"] == null ||
                !byte.TryParse(
                    Convert.ToString(Session["TipKorisnika"]),
                    out tipKorisnika) ||
                tipKorisnika != (byte)TipKorisnika.Zaposleni)
            {
                return new HttpStatusCodeResult(403);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var student = new StudentKlasa
                {
                    Ime = model.Ime,
                    Prezime = model.Prezime,
                    DatumRodjenja = model.DatumRodjenja,
                    Email = model.Email,
                    Telefon = model.Telefon,
                    BrojIndeksa = model.BrojIndeksa,
                    StudijskiProgram = model.StudijskiProgram,
                    GodinaStudija = model.GodinaStudija,
                    BezRoditelja = model.BezRoditelja,
                    Prosek = model.Prosek,

                    DokumentacijaPrihodaDostavljena =
                        model.DokumentacijaPrihodaDostavljenaChecked,

                    UkupnaPrimanjaDomacinstva =
                        model.UkupnaPrimanjaDomacinstva,

                    BrojClanovaPorodice =
                        model.BrojClanovaPorodice
                };

                string stringKonekcije =
                    ConfigurationManager
                        .ConnectionStrings["Konekcija"]
                        .ConnectionString;

                string adresaServisa =
                    ConfigurationManager
                        .AppSettings["OgranicenjaApiUrl"];

                IStudentRepository repo =
                    new StudentRepositorySP(stringKonekcije);

                IOgranicenjaKlijent ogranicenjaKlijent =
                    new OgranicenjaRestKlijent(adresaServisa);

                var servis = new StudentPrijavaServis(
                    repo,
                    ogranicenjaKlijent);

                int noviStudentID =
                    await servis.DodajStudentaAsync(student);

                TempData["Poruka"] =
                    "Prijava je uspešno sačuvana. ID prijave: " +
                    noviStudentID;

                return RedirectToAction("Index", "Zaposleni");
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                ModelState.AddModelError(
                    "",
                    "Servis za parametre konkursa trenutno nije dostupan. " +
                    "Pokušajte ponovo kasnije.");
            }
            catch (TaskCanceledException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                ModelState.AddModelError(
                    "",
                    "Servis za parametre konkursa nije odgovorio na vreme. " +
                    "Pokušajte ponovo.");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (SqlException ex)
                when (ex.Number >= 50040 &&
                      ex.Number <= 50042)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                ModelState.AddModelError(
                    "",
                    "Dogodila se greška prilikom čuvanja prijave.");
            }

            return View(model);
        }
    }
}
