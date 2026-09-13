using KlasePodataka;
using KorisnickiInterfejsMVC.Models;
using PoslovnaLogika;
using Repozitorijumi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;



namespace KorisnickiInterfejsMVC.Controllers
{
    public class AdminController : Controller
    {
        protected override void OnActionExecuting(
    ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (Session["KorisnikID"] == null ||
                Session["TipKorisnika"] == null ||
                Convert.ToByte(Session["TipKorisnika"]) !=
                    (byte)TipKorisnika.Administrator)
            {
                filterContext.Result =
                    RedirectToAction(
                        "UlogujAdmin",   
                        "Nalog");
            }
        }

        string konekcija = ConfigurationManager
            .ConnectionStrings["Konekcija"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RangLista(
    string godinaStudija,
    bool? isEligible)
        {
            List<StudentRankingVM> model =
                KreirajModelRangListe(
                    godinaStudija,
                    isEligible);

            ViewBag.GodinaStudija =
                godinaStudija;

            ViewBag.IsEligible =
                isEligible;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ObjaviPreliminarnuRangListu(
    string godinaStudija,
    bool? isEligible)
        {
            if (Session["KorisnikID"] == null ||
                Session["TipKorisnika"] == null)
            {
                return RedirectToAction(
                    "UlogujAdmin",
                    "Nalog");
            }

            byte tipKorisnika =
                Convert.ToByte(
                    Session["TipKorisnika"]);

            if (tipKorisnika !=
                (byte)TipKorisnika.Administrator)
            {
                TempData["Poruka"] =
                    "Samo administrator može da objavi preliminarnu rang-listu.";

                return RedirectToAction(
                    "Index",
                    "Home");
            }

            try
            {
                List<StudentRankingVM> model =
                    KreirajModelRangListe(
                        godinaStudija,
                        isEligible);

                List<PreliminarnaRangListaKlasa> stavke =
                    model
                        .Select((student, indeks) =>
                            new PreliminarnaRangListaKlasa
                            {
                                Pozicija = indeks + 1,
                                StudentID = student.ID,

                                Ime = student.Ime,
                                Prezime = student.Prezime,
                                BrojIndeksa =
                                    student.BrojIndeksa,

                                StudijskiProgram =
                                    student.StudijskiProgram,

                                GodinaStudija =
                                    student.GodinaStudija,

                                Prosek = student.Prosek,

                                BezRoditelja =
                                    student.BezRoditelja,

                                DokumentacijaPrihodaDostavljena =
                                    student
                                        .DokumentacijaPrihodaDostavljena,

                                UkupnaPrimanjaDomacinstva =
                                    student
                                        .UkupnaPrimanjaDomacinstva,

                                UkupnoBodova =
                                    student.UkupnoBodova
                            })
                        .ToList();

                IPreliminarnaRangListaRepository repo =
                    new PreliminarnaRangListaRepositorySP(
                        konekcija);

                PreliminarnaRangListaServis servis =
                    new PreliminarnaRangListaServis(
                        repo);

                int administratorID =
                    Convert.ToInt32(
                        Session["KorisnikID"]);

                servis.Objavi(
                    stavke,
                    administratorID);

                TempData["Poruka"] =
                    "Preliminarna rang-lista je uspešno objavljena.";
            }
            catch (Exception ex)
            {
                TempData["Poruka"] =
                    "Objavljivanje nije uspelo: " +
                    ex.Message;
            }

            return RedirectToAction(
                "RangLista",
                new
                {
                    godinaStudija = godinaStudija,
                    isEligible = isEligible
                });
        }


        public ActionResult StampajRangListu(string godinaStudija, bool? isEligible)
        {
            IStudentRepository repo = new StudentRepositorySP(konekcija);
            EvidencijaStudenataKlasa logika = new EvidencijaStudenataKlasa(repo, KreirajSkoringServis());
            var lista = logika.Rangiraj(new StudentFilter { GodinaStudija = godinaStudija, IsEligible = isEligible });

            var model = MapirajRangListu(lista);

            ViewBag.GodinaStudija = godinaStudija;
            ViewBag.IsEligible = isEligible;

            return View(model);
        }

        private System.Collections.Generic.List<StudentRankingVM> MapirajRangListu(
            System.Collections.Generic.List<KlasePodataka.StudentKlasa> lista)
        {
            return lista.Select(x => new StudentRankingVM
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
                UkupnaPrimanjaDomacinstva = x.UkupnaPrimanjaDomacinstva,
                BrojClanovaPorodice = x.BrojClanovaPorodice,
                BodoviProsek = x.BodoviProsek,
                BodoviPrimanja = x.BodoviPrimanja,
                BodoviGodina = x.BodoviGodina,
                UkupnoBodova = x.UkupnoBodova,
                IsEligible = x.IsEligible
            }).ToList();
        }

        [HttpGet]
        public ActionResult DodajKorisnika()
        {
            return View(new DodajKorisnikaVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajKorisnika(DodajKorisnikaVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                IKorisnikRepository repo =
                    new KorisnikRepositorySP(konekcija);

                KorisnikServis servis =
                    new KorisnikServis(repo);

                KorisnikKlasa korisnik = new KorisnikKlasa
                {
                    Ime = model.Ime,
                    Prezime = model.Prezime,
                    KorisnickoIme = model.KorisnickoIme,
                    TipKorisnika = model.TipKorisnika.Value,
                    Aktivan = true
                };

                int noviKorisnikID =
                    servis.DodajKorisnika(
                        korisnik,
                        model.Lozinka);

                TempData["Poruka"] =
                    "Korisnik je uspešno dodat. ID: " +
                    noviKorisnikID;

                return RedirectToAction("DodajKorisnika");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "Dogodila se greška prilikom dodavanja korisnika.");
            }

            return View(model);
        }

        private List<StudentRankingVM>
    KreirajModelRangListe(
        string godinaStudija,
        bool? isEligible)
        {
            IStudentRepository repo =
                new StudentRepositorySP(
                    konekcija);

            EvidencijaStudenataKlasa logika =
                new EvidencijaStudenataKlasa(repo,KreirajSkoringServis());

            var lista =
                logika.Rangiraj(
                    new StudentFilter
                    {
                        GodinaStudija =
                            godinaStudija,

                        IsEligible =
                            isEligible
                    });

            return MapirajRangListu(lista);
        }
        [HttpGet]
        public ActionResult PrimedbeNaRangListu()
        {
            List<AdministratorskaPrimedbaVM> model =
                KreirajServisPrimedbi()
                    .DajSve()
                    .Select(x =>
                        new AdministratorskaPrimedbaVM
                        {
                            ID = x.ID,

                            Ime = x.Ime,
                            Prezime = x.Prezime,
                            Email = x.Email,
                            Komentar = x.Komentar,

                            DatumPodnosenja =
                                x.DatumPodnosenja,

                            Obradjena =
                                x.Obradjena,

                            DatumObrade =
                                x.DatumObrade,

                            ObradioKorisnikID =
                                x.ObradioKorisnikID,

                            ObradioImeIPrezime =
                                x.ObradioImeIPrezime,

                            ObradioKorisnickoIme =
                                x.ObradioKorisnickoIme
                        })
                    .ToList();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OznaciPrimedbuObradjenom(
    int primedbaID)
        {
            if (Session["KorisnikID"] == null ||
                Session["TipKorisnika"] == null)
            {
                return RedirectToAction(
                    "UlogujAdmin",
                    "Nalog");
            }

            byte tipKorisnika =
                Convert.ToByte(
                    Session["TipKorisnika"]);

            if (tipKorisnika !=
                (byte)TipKorisnika.Administrator)
            {
                TempData["Poruka"] =
                    "Samo administrator može da obrađuje primedbe.";

                return RedirectToAction(
                    "Index",
                    "Home");
            }

            try
            {
                int administratorID =
                    Convert.ToInt32(
                        Session["KorisnikID"]);

                KreirajServisPrimedbi()
                    .OznaciObradjenom(
                        primedbaID,
                        administratorID);

                TempData["Poruka"] =
                    "Primedba je označena kao obrađena.";
            }
            catch (ArgumentException ex)
            {
                TempData["Poruka"] =
                    ex.Message;
            }
            catch (SqlException ex)
                when (ex.Number >= 50033 &&
                      ex.Number <= 50034)
            {
                TempData["Poruka"] =
                    ex.Message;
            }
            catch (Exception)
            {
                TempData["Poruka"] =
                    "Dogodila se greška pri obradi primedbe.";
            }

            return RedirectToAction(
                "PrimedbeNaRangListu");
        }

        private PrimedbaNaRangListuServis
    KreirajServisPrimedbi()
        {
            IPrimedbaNaRangListuRepository repo =
                new PrimedbaNaRangListuRepositorySP(
                    konekcija);

            return new PrimedbaNaRangListuServis(
                repo);
        }


        private bool MozeDaUpravljaSkoringom()
        {
            if (Session["KorisnikID"] == null ||
                Session["TipKorisnika"] == null)
            {
                return false;
            }

            byte tip;

            return byte.TryParse(
                       Convert.ToString(Session["TipKorisnika"]),
                       out tip)
                   && tip == (byte)TipKorisnika.Administrator;
        }

        private SkoringKonfiguracijaServis KreirajSkoringServis()
        {
            ISkoringKonfiguracijaRepository repo =
                new SkoringKonfiguracijaRepositoryEF(konekcija);

            return new SkoringKonfiguracijaServis(repo);
        }

        [HttpGet]
        public ActionResult UpravljanjeSkoringom()
        {
            if (!MozeDaUpravljaSkoringom())
            {
                return new HttpStatusCodeResult(403);
            }

            try
            {
                var konfiguracija =
                    KreirajSkoringServis().DajKonfiguraciju();

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
                    BezRoditeljaMultiplier =
                        konfiguracija.BezRoditeljaMultiplier
                };

                return View(model);
            }
            catch (ArgumentException ex)
            {
                TempData["Poruka"] = ex.Message;
            }
            catch (SqlException ex)
                when (ex.Number >= 50050 && ex.Number <= 50059)
            {
                TempData["Poruka"] = ex.Message;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                TempData["Poruka"] =
                    "Podešavanja bodovanja trenutno nije moguće učitati.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpravljanjeSkoringom(
            SkoringKonfiguraciaVM model)
        {
            if (!MozeDaUpravljaSkoringom())
            {
                return new HttpStatusCodeResult(403);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var servis = KreirajSkoringServis();

                // ID uzimamo iz baze, jer koristimo jednu konfiguraciju.
                var postojecaKonfiguracija = servis.DajKonfiguraciju();

                var konfiguracija = new SkoringKonfiguracijaKlasa
                {
                    ID = postojecaKonfiguracija.ID,
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

                servis.SacuvajKonfiguraciju(konfiguracija);

                TempData["Poruka"] =
                    "Konfiguracija bodovanja je uspešno sačuvana.";

                return RedirectToAction("UpravljanjeSkoringom");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (SqlException ex)
                when (ex.Number >= 50050 && ex.Number <= 50059)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                ModelState.AddModelError(
                    "",
                    "Konfiguraciju trenutno nije moguće sačuvati.");
            }

            return View(model);
        }

    }

}
