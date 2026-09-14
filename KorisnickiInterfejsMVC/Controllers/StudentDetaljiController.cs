using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using KlasePodataka;
using KorisnickiInterfejsMVC.Models;
using PoslovnaLogika;
using Repozitorijumi;
using System.Data.SqlClient;
using PrezentacionaLogika;

namespace KorisnickiInterfejsMVC.Controllers
{
    public class StudentDetaljiController : Controller
    {
        protected override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (Session["KorisnikID"] == null ||
                Session["TipKorisnika"] == null)
            {
                filterContext.Result =
                    RedirectToAction("UlogujAdmin", "Nalog");

                return;
            }

            byte tip;

            if (!byte.TryParse(
                    Convert.ToString(Session["TipKorisnika"]),
                    out tip) ||
                (tip != (byte)TipKorisnika.Administrator &&
                 tip != (byte)TipKorisnika.Zaposleni))
            {
                filterContext.Result =
                    new HttpStatusCodeResult(403);
            }
        }

        
        [HttpGet]
        public ActionResult Index(int? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                return new HttpStatusCodeResult(
                    400,
                    "Neispravan ID studenta.");
            }

            StudentDetaljiVM model = KreirajModel(id.Value);

            if (model == null)
            {
                return HttpNotFound("Student nije pronađen.");
            }

            ViewBag.JeZaposleni = JeZaposleni();

            return View(model);
        }

        [HttpGet]
        public ActionResult Stampaj(int? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                return new HttpStatusCodeResult(
                    400,
                    "Neispravan ID studenta.");
            }

            StudentDetaljiVM model = KreirajModel(id.Value);

            if (model == null)
            {
                return HttpNotFound("Student nije pronađen.");
            }

            return View(model);
        }

        private StudentDetaljiVM KreirajModel(int studentID)
        {
            string konekcija =
                ConfigurationManager
                    .ConnectionStrings["Konekcija"]
                    .ConnectionString;

            var forma = new FormaStudentDetaljiKlasa(
                new StudentRepositorySP(konekcija),
                new StudentDokumentRepositorySP(konekcija));

            var detalji = forma.DajDetalje(studentID);

            if (detalji == null)
            {
                return null;
            }

            var student = detalji.Student;

            return new StudentDetaljiVM
            {
                StudentID = student.ID,
                Ime = student.Ime,
                Prezime = student.Prezime,
                DatumRodjenja = student.DatumRodjenja,
                Email = student.Email,
                Telefon = student.Telefon,
                BrojIndeksa = student.BrojIndeksa,
                StudijskiProgram = student.StudijskiProgram,
                GodinaStudija = student.GodinaStudija,
                Prosek = student.Prosek,
                BezRoditelja = student.BezRoditelja,
                UkupnaPrimanjaDomacinstva =
                    student.UkupnaPrimanjaDomacinstva,
                BrojClanovaPorodice = student.BrojClanovaPorodice,

                Dokumenti = detalji.Dokumenti
                    .Select(d => new StudentDokumentVM
                    {
                        ID = d.ID,
                        StudentID = d.StudentID,
                        VrstaDokumentaID = d.VrstaDokumentaID,
                        NazivDokumenta = d.NazivDokumenta,
                        Obavezan = d.Obavezan,
                        Podnet = d.Podnet,
                        DatumPodnosenja = d.DatumPodnosenja,
                        Ispravan = d.Ispravan,
                        DatumProvere = d.DatumProvere,
                        ProverioImeIPrezime = d.ProverioImeIPrezime,
                        Napomena = d.Napomena
                    })
                    .ToList()
            };
        }

        private bool JeZaposleni()
        {
            byte tip;

            return Session["KorisnikID"] != null
                && byte.TryParse(
                    Convert.ToString(Session["TipKorisnika"]),
                    out tip)
                && tip == (byte)TipKorisnika.Zaposleni;
        }

        private StudentDokumentServis KreirajDokumentServis()
        {
            string konekcija =
                ConfigurationManager
                    .ConnectionStrings["Konekcija"]
                    .ConnectionString;

            return new StudentDokumentServis(
                new StudentDokumentRepositorySP(konekcija));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OznaciPodnetim(
            int studentID,
            int dokumentID)
        {
            if (!JeZaposleni())
            {
                return new HttpStatusCodeResult(403);
            }

            if (!ModelState.IsValid ||
                studentID <= 0 ||
                dokumentID <= 0)
            {
                return new HttpStatusCodeResult(400);
            }

            return IzvrsiIzmenuDokumenta(
                studentID,
                () => KreirajDokumentServis()
                    .OznaciPodnetim(studentID, dokumentID),
                "Dokument je označen kao podnet.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProveriDokument(
            int studentID,
            int dokumentID,
            bool? ispravan,
            string napomena)
        {
            if (!JeZaposleni())
            {
                return new HttpStatusCodeResult(403);
            }

            if (studentID <= 0 || dokumentID <= 0)
            {
                return new HttpStatusCodeResult(400);
            }

            if (!ModelState.IsValid)
            {
                TempData["Poruka"] =
                    "Podaci za proveru dokumenta nisu ispravni.";

                return RedirectToAction("Index", new { id = studentID });
            }

            int korisnikID;

            if (!int.TryParse(
                    Convert.ToString(Session["KorisnikID"]),
                    out korisnikID) ||
                korisnikID <= 0)
            {
                return new HttpStatusCodeResult(403);
            }

            return IzvrsiIzmenuDokumenta(
                studentID,
                () => KreirajDokumentServis().ProveriDokument(
                    studentID,
                    dokumentID,
                    ispravan,
                    korisnikID,
                    napomena),
                "Rezultat provere dokumenta je sačuvan.");
        }

        private ActionResult IzvrsiIzmenuDokumenta(
            int studentID,
            Action izmena,
            string poruka)
        {
            try
            {
                izmena();
                TempData["Poruka"] = poruka;
            }
            catch (ArgumentException ex)
            {
                TempData["Poruka"] = ex.Message;
            }
            catch (SqlException ex)
                when (ex.Number >= 50063 && ex.Number <= 50069)
            {
                TempData["Poruka"] = ex.Message;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                TempData["Poruka"] =
                    "Dogodila se greška pri obradi dokumenta.";
            }

            return RedirectToAction("Index", new { id = studentID });
        }
    }
}