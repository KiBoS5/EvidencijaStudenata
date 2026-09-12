using KlasePodataka;
using KorisnickiInterfejsMVC.Models;
using PoslovnaLogika;
using PrezentacionaLogika;
using Repozitorijumi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace KorisnickiInterfejsMVC.Controllers
{
    public class AdminController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["admin"] == null)
            {
                filterContext.Result =
                    RedirectToAction("LoginAdmin", "Account");
            }

            base.OnActionExecuting(filterContext);
        }

        string konekcija = ConfigurationManager
            .ConnectionStrings["Konekcija"].ConnectionString;

        // ...existing code...

        public ActionResult RangLista(string godinaStudija, bool? isEligible)
        {
            IStudentRepository repo = new StudentRepositorySP(konekcija);
            EvidencijaStudenataKlasa logika = new EvidencijaStudenataKlasa(repo, konekcija);
            var lista = logika.Rangiraj(new StudentFilter { GodinaStudija = godinaStudija, IsEligible = isEligible });

            var model = lista.Select(x => new StudentRankingVM
            {
                ID = x.ID,
                Ime = x.Ime,
                Prezime = x.Prezime,
                BrojIndeksa = x.BrojIndeksa,
                StudijskiProgram = x.StudijskiProgram,
                GodinaStudija = x.GodinaStudija,
                Prosek = x.Prosek,
                BezRoditelja = x.BezRoditelja,
                DokumentacijaPrihodaDostavljena = x.DokumentacijaPrihodaDostavljena,
                UkupnaPrimanjaDoma?instva = x.UkupnaPrimanjaDoma?instva,
                Broj?lanovaPorodice = x.Broj?lanovaPorodice,
                BodoviProsek = x.BodoviProsek,
                BodoviPrimanja = x.BodoviPrimanja,
                BodoviGodina = x.BodoviGodina,
                UkupnoBodova = x.UkupnoBodova,
                IsEligible = x.IsEligible
            }).ToList();

            ViewBag.GodinaStudija = godinaStudija;
            ViewBag.IsEligible = isEligible;

            return View(model);
        }

        public ActionResult ExportRangLista()
        {
            IStudentRepository repo = new StudentRepositorySP(konekcija);
            EvidencijaStudenataKlasa logika = new EvidencijaStudenataKlasa(repo, konekcija);
            var lista = logika.Rangiraj();

            // Enhanced CSV with average income per family member
            var csv = "RedniBroj;Ime;Prezime;BrojIndeksa;StudijskiProgram;GodinaStudija;Prosek;BezRoditelja;DokumentacijaPrihodaDostavljena;UkupnaPrimanja;Broj?lanova;Prose?naPrimanjaPo?lanu;BodoviProsek;BodoviPrimanja;BodoviGodina;UkupnoBodova\n";
            int redniBroj = 1;

            foreach (var x in lista)
            {
                decimal prose?naPrimanja = x.Broj?lanovaPorodice > 0 ? x.UkupnaPrimanjaDoma?instva / x.Broj?lanovaPorodice : 0;
                csv += $"{redniBroj};{x.Ime};{x.Prezime};{x.BrojIndeksa};{x.StudijskiProgram};{x.GodinaStudija};{x.Prosek};{x.BezRoditelja};{x.DokumentacijaPrihodaDostavljena};{x.UkupnaPrimanjaDoma?instva};{x.Broj?lanovaPorodice};{prose?naPrimanja};{x.BodoviProsek};{x.BodoviPrimanja};{x.BodoviGodina};{x.UkupnoBodova}\n";
                redniBroj++;
            }

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv", "RangListaStudenata.csv");
        }

        /// <summary>
        /// Document verification page - shows students needing document verification
        /// </summary>
        public ActionResult ProveraDocumentacije()
        {
            FormaStudentKlasa formaStudent = new FormaStudentKlasa(konekcija);
            FormaDokumentacijeStudentaKlasa formaDokumentacija = new FormaDokumentacijeStudentaKlasa(konekcija);

            var studenti = formaStudent.DajSveStudente();
            var model = new List<StudentDokumentaciaVM>();

            foreach (var student in studenti)
            {
                var dokumentacija = formaDokumentacija.DajDokumentacijuStudenta(student.ID);

                model.Add(new StudentDokumentaciaVM
                {
                    ID = dokumentacija?.ID ?? 0,
                    StudentID = student.ID,
                    StudentIme = student.Ime,
                    StudentPrezime = student.Prezime,
                    BrojIndeksa = student.BrojIndeksa,
                    SviDokumentiPodneseni = dokumentacija?.SviDokumentiPodneseni ?? false,
                    DatumPodnosenja = dokumentacija?.DatumPodnosenja,
                    SviDokumentiProvereni = dokumentacija?.SviDokumentiProvereni ?? false,
                    DatumProvere = dokumentacija?.DatumProvere,
                    NapomenaProvere = dokumentacija?.NapomenaProvere,
                    SpremnaZaUnos = dokumentacija?.SpremnaZaUnos ?? false
                });
            }

            return View(model);
        }

        /// <summary>
        /// Verify documents for a student (admin action)
        /// </summary>
        [HttpPost]
        public ActionResult ProveridokumentacijuStudenta(int studentID, string napomena)
        {
            FormaDokumentacijeStudentaKlasa forma = new FormaDokumentacijeStudentaKlasa(konekcija);
            bool rezultat = forma.ProveridokumentacijuStudenta(studentID, napomena);

            if (rezultat)
                TempData["Poruka"] = "Dokumentacija studenta je proverena!";
            else
                TempData["Poruka"] = "Greška pri proveri dokumentacije!";

            return RedirectToAction("ProveraDocumentacije");
        }

        /// <summary>
        /// Mark student as ready for data entry
        /// </summary>
        [HttpPost]
        public ActionResult Ozna?iSpremnaZaUnos(int studentID)
        {
            FormaDokumentacijeStudentaKlasa forma = new FormaDokumentacijeStudentaKlasa(konekcija);
            bool rezultat = forma.Ozna?iSpremnaZaUnos(studentID);

            if (rezultat)
                TempData["Poruka"] = "Student je ozna?en kao spreman za unos!";
            else
                TempData["Poruka"] = "Greška pri ozna?avanju studenta!";

            return RedirectToAction("ProveraDocumentacije");
        }

        /// <summary>
        /// Manage scoring configuration
        /// </summary>
        public ActionResult UpravljanjeSkoringom()
        {
            FormaSkoringKonfiguracieKlasa forma = new FormaSkoringKonfiguracieKlasa(konekcija);
            var konfiguracija = forma.DajKonfiguraciju();

            var model = new SkoringKonfiguraciaVM
            {
                ID = konfiguracija.ID,
                MinimalanProsek = konfiguracija.MinimalanProsek,
                ProsekZa10Bodova = konfiguracija.ProsekZa10Bodova,
                ProsekZa15Bodova = konfiguracija.ProsekZa15Bodova,
                ProsekZa20Bodova = konfiguracija.ProsekZa20Bodova,
                PrimanjaZa10Bodova = konfiguracija.PrimanjaZa10Bodova,
                PrimanjaZa1Bod = konfiguracija.PrimanjaZa1Bod,
                BodoviGodinaStudije1 = konfiguracija.BodoviGodinaStudije1,
                BodoviGodinaStudije2 = konfiguracija.BodoviGodinaStudije2,
                BodoviGodinaStudije3 = konfiguracija.BodoviGodinaStudije3,
                BodoviGodinaStudije4 = konfiguracija.BodoviGodinaStudije4,
                BodoviMaster = konfiguracija.BodoviMaster,
                BezRoditeljaMultiplier = konfiguracija.BezRoditeljaMultiplier
            };

            return View(model);
        }

        /// <summary>
        /// Update scoring configuration
        /// </summary>
        [HttpPost]
        public ActionResult UpravljanjeSkoringom(SkoringKonfiguraciaVM model)
        {
            FormaSkoringKonfiguracieKlasa forma = new FormaSkoringKonfiguracieKlasa(konekcija);

            var konfiguracija = new SkoringKonfiguraciaKlasa
            {
                ID = model.ID,
                MinimalanProsek = model.MinimalanProsek,
                ProsekZa10Bodova = model.ProsekZa10Bodova,
                ProsekZa15Bodova = model.ProsekZa15Bodova,
                ProsekZa20Bodova = model.ProsekZa20Bodova,
                PrimanjaZa10Bodova = model.PrimanjaZa10Bodova,
                PrimanjaZa1Bod = model.PrimanjaZa1Bod,
                BodoviGodinaStudije1 = model.BodoviGodinaStudije1,
                BodoviGodinaStudije2 = model.BodoviGodinaStudije2,
                BodoviGodinaStudije3 = model.BodoviGodinaStudije3,
                BodoviGodinaStudije4 = model.BodoviGodinaStudije4,
                BodoviMaster = model.BodoviMaster,
                BezRoditeljaMultiplier = model.BezRoditeljaMultiplier
            };

            bool rezultat = forma.AžurirajKonfiguraciju(konfiguracija);

            if (rezultat)
                TempData["Poruka"] = "Konfiguracija skoringa je ažurirana!";
            else
                TempData["Poruka"] = "Greška pri ažuriranju konfiguracije!";

            return RedirectToAction("UpravljanjeSkoringom");
        }
    }
}
