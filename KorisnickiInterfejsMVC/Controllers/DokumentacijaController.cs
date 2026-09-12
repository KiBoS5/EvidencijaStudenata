using System;
using System.Configuration;
using System.Data.SqlClient;
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
                    RedirectToAction(
                        "UlogujAdmin",
                        "Nalog");

                return;
            }

            byte tipKorisnika =
                Convert.ToByte(
                    Session["TipKorisnika"]);

            bool dozvoljenPristup =
                tipKorisnika ==
                    (byte)TipKorisnika.Administrator ||
                tipKorisnika ==
                    (byte)TipKorisnika.Zaposleni;

            if (!dozvoljenPristup)
            {
                filterContext.Result =
                    RedirectToAction(
                        "UlogujAdmin",
                        "Nalog");
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            StudentDokumentacijaServis servis =
                KreirajServis();

            var model = servis
                .DajDokumentacijuStudenata()
                .Select(x =>
                    new StudentDokumentacijaVM
                    {
                        DokumentacijaID = x.ID,
                        StudentID = x.StudentID,

                        StudentIme = x.StudentIme,
                        StudentPrezime =
                            x.StudentPrezime,

                        BrojIndeksa = x.BrojIndeksa,

                        SviDokumentiPodneseni =
                            x.SviDokumentiPodneseni,

                        DatumPodnosenja =
                            x.DatumPodnosenja,

                        SviDokumentiProvereni =
                            x.SviDokumentiProvereni,

                        DatumProvere =
                            x.DatumProvere,

                        DokumentacijaIspravna =
                            x.DokumentacijaIspravna,

                        NapomenaProvere =
                            x.NapomenaProvere,

                        SpremnaZaUnos =
                            x.SpremnaZaUnos,

                        ProverioKorisnikID =
                            x.ProverioKorisnikID,

                        ProverioImeIPrezime =
                            x.ProverioImeIPrezime,

                        ProverioKorisnickoIme =
                            x.ProverioKorisnickoIme,

                        StanjeProcesa =
                            x.StanjeProcesa
                    })
                .ToList();

            ViewBag.JeZaposleni =
                DaLiJeZaposleni();

            ViewBag.JeAdministrator =
                DaLiJeAdministrator();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OznaciDokumentacijuPodnetom(
            int studentID)
        {
            if (!DaLiJeZaposleni())
            {
                return ZabraniIzmenu();
            }

            return IzvrsiAkciju(
                () => KreirajServis()
                    .OznaciDokumentacijuPodnetom(
                        studentID),

                "Dokumentacija je označena kao podneta.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProveriDokumentacijuStudenta(
            int studentID,
            bool dokumentacijaIspravna,
            string napomena)
        {
            if (!DaLiJeZaposleni())
            {
                return ZabraniIzmenu();
            }

            int prijavljeniKorisnikID =
                Convert.ToInt32(
                    Session["KorisnikID"]);

            return IzvrsiAkciju(
                () => KreirajServis()
                    .ProveriDokumentacijuStudenta(
                        studentID,
                        dokumentacijaIspravna,
                        napomena,
                        prijavljeniKorisnikID),

                dokumentacijaIspravna
                    ? "Dokumentacija je proverena i ispravna."
                    : "Dokumentacija je proverena, ali nije ispravna.");
        }

        private StudentDokumentacijaServis
            KreirajServis()
        {
            IStudentDokumentacijaRepository repo =
                new StudentDokumentacijaRepositorySP(
                    _konekcija);

            return new StudentDokumentacijaServis(
                repo);
        }

        private bool DaLiJeZaposleni()
        {
            if (Session["TipKorisnika"] == null)
            {
                return false;
            }

            byte tipKorisnika =
                Convert.ToByte(
                    Session["TipKorisnika"]);

            return tipKorisnika ==
                (byte)TipKorisnika.Zaposleni;
        }

        private bool DaLiJeAdministrator()
        {
            if (Session["TipKorisnika"] == null)
            {
                return false;
            }

            byte tipKorisnika =
                Convert.ToByte(
                    Session["TipKorisnika"]);

            return tipKorisnika ==
                (byte)TipKorisnika.Administrator;
        }

        private ActionResult ZabraniIzmenu()
        {
            TempData["Poruka"] =
                "Samo zaposleni može da obrađuje dokumentaciju.";

            return RedirectToAction("Index");
        }

        private ActionResult IzvrsiAkciju(
            Action akcija,
            string poruka)
        {
            try
            {
                akcija();
                TempData["Poruka"] = poruka;
            }
            catch (ArgumentException ex)
            {
                TempData["Poruka"] = ex.Message;
            }
            catch (SqlException ex)
                when (ex.Number >= 50010 &&
                      ex.Number <= 50014)
            {
                TempData["Poruka"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["Poruka"] =
                    "Dogodila se greška pri obradi dokumentacije.";
            }

            return RedirectToAction("Index");
        }
    }
}