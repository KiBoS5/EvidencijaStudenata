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

        private bool JeZaposleni()
        {
            int korisnikID;
            byte tipKorisnika;

            return int.TryParse(
                       Convert.ToString(Session["KorisnikID"]),
                       out korisnikID)
                   && korisnikID > 0
                   && byte.TryParse(
                       Convert.ToString(Session["TipKorisnika"]),
                       out tipKorisnika)
                   && tipKorisnika == (byte)TipKorisnika.Zaposleni;
        }

        private IStudentRepository KreirajRepozitorijum()
        {
            string konekcija =
                ConfigurationManager
                    .ConnectionStrings["Konekcija"]
                    .ConnectionString;

            return new StudentRepositorySP(konekcija);
        }

        [HttpGet]
        public ActionResult Izmeni(int? id)
        {
            if (!JeZaposleni())
            {
                return new HttpStatusCodeResult(403);
            }

            if (!id.HasValue || id.Value <= 0)
            {
                return new HttpStatusCodeResult(
                    400,
                    "Neispravan ID studenta.");
            }

            StudentKlasa student =
                KreirajRepozitorijum().DajStudentaPoID(id.Value);

            if (student == null)
            {
                return HttpNotFound("Student nije pronađen.");
            }

            var model = new StudentPrijavljivanjeVM
            {
                Ime = student.Ime,
                Prezime = student.Prezime,
                DatumRodjenja = student.DatumRodjenja,
                Email = student.Email,
                Telefon = student.Telefon,
                BrojIndeksa = student.BrojIndeksa,
                StudijskiProgram = student.StudijskiProgram,
                GodinaStudija = student.GodinaStudija,
                BezRoditelja = student.BezRoditelja,
                Prosek = student.Prosek,
                UkupnaPrimanjaDomacinstva =
                    student.UkupnaPrimanjaDomacinstva,
                BrojClanovaPorodice = student.BrojClanovaPorodice
            };

            ViewBag.StudentID = student.ID;

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Izmeni(
    int id,
    StudentPrijavljivanjeVM model)
        {
            if (!JeZaposleni())
            {
                return new HttpStatusCodeResult(403);
            }

            if (id <= 0)
            {
                return new HttpStatusCodeResult(
                    400,
                    "Neispravan ID studenta.");
            }

            ViewBag.StudentID = id;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var student = new StudentKlasa
                {
                    ID = id,
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
                    UkupnaPrimanjaDomacinstva =
                        model.UkupnaPrimanjaDomacinstva,
                    BrojClanovaPorodice = model.BrojClanovaPorodice
                };

                var servis = new StudentIzmenaServis(
                    KreirajRepozitorijum());

                servis.IzmeniStudenta(student);

                TempData["Poruka"] =
                    "Prijava je izmenjena. Dokumentaciju je potrebno ponovo proveriti.";

                return RedirectToAction(
                    "Index",
                    "StudentDetalji",
                    new { id = id });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (SqlException ex)
                when (ex.Number >= 50070 && ex.Number <= 50075)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                ModelState.AddModelError(
                    "",
                    "Dogodila se greška prilikom izmene prijave.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Obrisi(int studentID)
        {
            if (!JeZaposleni())
            {
                return new HttpStatusCodeResult(403);
            }

            if (!ModelState.IsValid || studentID <= 0)
            {
                return new HttpStatusCodeResult(
                    400,
                    "Neispravan ID studenta.");
            }

            try
            {
                var servis = new StudentIzmenaServis(
                    KreirajRepozitorijum());

                servis.ObrisiStudenta(studentID);

                TempData["Poruka"] =
                    "Prijava i pripadajuća dokumentacija su obrisane.";
            }
            catch (ArgumentException ex)
            {
                TempData["Poruka"] = ex.Message;
            }
            catch (SqlException ex)
                when (ex.Number >= 50070 && ex.Number <= 50075)
            {
                TempData["Poruka"] = ex.Message;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                TempData["Poruka"] =
                    "Dogodila se greška prilikom brisanja prijave.";
            }

            return RedirectToAction("Index", "Dokumentacija");
        }
    }
}
