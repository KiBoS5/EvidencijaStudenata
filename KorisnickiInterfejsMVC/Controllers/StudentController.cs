using KlasePodataka;
using KorisnickiInterfejsMVC.Models;
using Repozitorijumi;
using Servisi;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
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
        public ActionResult Prijava(
    StudentPrijavljivanjeVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                OgranicenjaServis ogranicenjaServis =
                    new OgranicenjaServis();

                if (!ogranicenjaServis.KonkursJeOtvoren())
                {
                    OgranicenjaKonkursaKlasa ogranicenja =
                        ogranicenjaServis.DajOgranicenja();

                    ModelState.AddModelError(
                        "",
                        string.Format(
                            "Prijave su dozvoljene od {0} do {1}.",
                            ogranicenja
                                .DatumPocetkaKonkursa
                                .ToString("dd.MM.yyyy HH:mm"),
                            ogranicenja
                                .DatumZavrsetkaKonkursa
                                .ToString("dd.MM.yyyy HH:mm")));

                    return View(model);
                }

                StudentKlasa student =
                    new StudentKlasa
                    {
                        Ime = model.Ime,
                        Prezime = model.Prezime,

                        DatumRodjenja =
                            model.DatumRodjenja,

                        Email = model.Email,
                        Telefon = model.Telefon,

                        BrojIndeksa =
                            model.BrojIndeksa,

                        StudijskiProgram =
                            model.StudijskiProgram,

                        GodinaStudija =
                            model.GodinaStudija,

                        BezRoditelja =
                            model.BezRoditelja,

                        Prosek = model.Prosek,

                        DokumentacijaPrihodaDostavljena =
                            model
                                .DokumentacijaPrihodaDostavljenaChecked,

                        UkupnaPrimanjaDomacinstva =
                            model
                                .UkupnaPrimanjaDomacinstva,

                        BrojClanovaPorodice =
                            model.BrojClanovaPorodice
                    };

                string stringKonekcije =
                    ConfigurationManager
                        .ConnectionStrings["Konekcija"]
                        .ConnectionString;

                IStudentRepository repo =
                    new StudentRepositorySP(
                        stringKonekcije);

                int maksimalanBrojPrijava =
                    ogranicenjaServis
                        .DajMaksimalanBrojPrijava();

                int noviStudentID =
                    repo.DodajStudenta(
                        student,
                        maksimalanBrojPrijava);

                TempData["Poruka"] =
                    "Prijava je uspešno sačuvana. ID prijave: " +
                    noviStudentID;

                return RedirectToAction(
                    "Index",
                    "Zaposleni");
            }
            catch (FileNotFoundException)
            {
                ModelState.AddModelError(
                    "",
                    "Fajl sa ograničenjima nije pronađen.");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);
            }
            catch (SqlException ex)
                when (ex.Number >= 50040 &&
                      ex.Number <= 50042)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "Dogodila se greška prilikom čuvanja prijave.");
            }

            return View(model);
        }
    }
}
