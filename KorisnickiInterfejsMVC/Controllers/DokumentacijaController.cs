using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using KlasePodataka;
using KorisnickiInterfejsMVC.Models;
using PoslovnaLogika;
using Repozitorijumi;

namespace KorisnickiInterfejsMVC.Controllers
{
    public class DokumentacijaController : Controller
    {
        private readonly string _konekcija =
            ConfigurationManager
                .ConnectionStrings["Konekcija"]
                .ConnectionString;

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

            byte tipKorisnika;

            if (!byte.TryParse(
                    Convert.ToString(Session["TipKorisnika"]),
                    out tipKorisnika))
            {
                filterContext.Result =
                    new HttpStatusCodeResult(403);

                return;
            }

            bool jeAdministrator =
                tipKorisnika == (byte)TipKorisnika.Administrator;

            bool jeZaposleni =
                tipKorisnika == (byte)TipKorisnika.Zaposleni;

            if (!jeAdministrator && !jeZaposleni)
            {
                filterContext.Result =
                    new HttpStatusCodeResult(403);

                return;
            }

            ViewBag.JeAdministrator = jeAdministrator;
            ViewBag.JeZaposleni = jeZaposleni;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var servis = KreirajServis();

            var model = servis
                .DajDokumentacijuStudenata()
                .Select(x => new StudentDokumentacijaVM
                {
                    DokumentacijaID = x.ID,
                    StudentID = x.StudentID,

                    StudentIme = x.StudentIme,
                    StudentPrezime = x.StudentPrezime,
                    BrojIndeksa = x.BrojIndeksa,

                    SviDokumentiPodneseni =
                        x.SviDokumentiPodneseni,

                    DatumPodnosenja = x.DatumPodnosenja,

                    SviDokumentiProvereni =
                        x.SviDokumentiProvereni,

                    DatumProvere = x.DatumProvere,

                    DokumentacijaIspravna =
                        x.DokumentacijaIspravna,

                    NapomenaProvere = x.NapomenaProvere,
                    SpremnaZaUnos = x.SpremnaZaUnos,

                    ProverioKorisnikID =
                        x.ProverioKorisnikID,

                    ProverioImeIPrezime =
                        x.ProverioImeIPrezime,

                    ProverioKorisnickoIme =
                        x.ProverioKorisnickoIme,

                    StanjeProcesa = x.StanjeProcesa
                })
                .ToList();

            return View(model);
        }

        private StudentDokumentacijaServis KreirajServis()
        {
            IStudentDokumentacijaRepository repo =
                new StudentDokumentacijaRepositorySP(_konekcija);

            return new StudentDokumentacijaServis(repo);
        }
    }
}