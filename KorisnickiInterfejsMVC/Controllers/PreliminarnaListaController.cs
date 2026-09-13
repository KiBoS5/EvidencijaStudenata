using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using KlasePodataka;
using KorisnickiInterfejsMVC.Models;
using PoslovnaLogika;
using Repozitorijumi;
using System.Net.Http;
using System.Threading.Tasks;
using Servisi;


namespace KorisnickiInterfejsMVC.Controllers
{
    public class PreliminarnaListaController : Controller
    {
        private readonly string _konekcija =
            ConfigurationManager
                .ConnectionStrings["Konekcija"]
                .ConnectionString;

        protected override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (Session["KorisnikID"] == null)
            {
                return;
            }

            if (Session["TipKorisnika"] == null)
            {
                filterContext.Result =
                    RedirectToAction(
                        "Index",
                        "Home");

                return;
            }

            byte tipKorisnika =
                Convert.ToByte(
                    Session["TipKorisnika"]);

            if (tipKorisnika ==
                (byte)TipKorisnika.Administrator)
            {
                filterContext.Result =
                    RedirectToAction(
                        "Index",
                        "Admin");

                return;
            }

            if (tipKorisnika ==
                (byte)TipKorisnika.Zaposleni)
            {
                filterContext.Result =
                    RedirectToAction(
                        "Index",
                        "Zaposleni");

                return;
            }

            filterContext.Result =
                RedirectToAction(
                    "Index",
                    "Home");
        }

        [HttpGet]
        public ActionResult Index()
        {
            PreliminarnaRangListaJavniVM model =
                KreirajJavniModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PodnesiPrimedbu(
            [Bind(Prefix = "Primedba")]
            PrimedbaNaRangListuVM primedba)
        {
            // Javni deo je namenjen neprijavljenim korisnicima.
            if (Session["KorisnikID"] != null)
            {
                return new HttpStatusCodeResult(403);
            }

            PreliminarnaRangListaJavniVM model =
                KreirajJavniModel();

            model.Primedba =
                primedba ?? new PrimedbaNaRangListuVM();

            if (primedba == null)
            {
                ModelState.AddModelError(
                    "",
                    "Podaci primedbe nisu dostavljeni.");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                var novaPrimedba = new PrimedbaNaRangListuKlasa
                {
                    Ime = primedba.Ime,
                    Prezime = primedba.Prezime,
                    Email = primedba.Email,
                    Komentar = primedba.Komentar
                };

                string adresaServisa =
                    ConfigurationManager
                        .AppSettings["OgranicenjaApiUrl"];

                IOgranicenjaKlijent ogranicenjaKlijent =
                    new OgranicenjaRestKlijent(adresaServisa);

                await KreirajServisPrimedbi().DodajAsync(
                    novaPrimedba,
                    model.ListaJeObjavljena,
                    ogranicenjaKlijent);

                TempData["Poruka"] =
                    "Primedba je uspešno podneta.";

                return RedirectToAction("Index");
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
                when (ex.Number >= 50030 &&
                      ex.Number <= 50032)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());

                ModelState.AddModelError(
                    "",
                    "Dogodila se greška pri podnošenju primedbe.");
            }

            return View("Index", model);
        }

        private PreliminarnaRangListaJavniVM
            KreirajJavniModel()
        {
            PreliminarnaRangListaJavniVM model =
                new PreliminarnaRangListaJavniVM();

            model.Stavke =
                KreirajServisRangListe()
                    .DajObjavljenuListu()
                    .Select(x =>
                        new PreliminarnaRangListaStavkaVM
                        {
                            Pozicija = x.Pozicija,

                            Ime = x.Ime,
                            Prezime = x.Prezime,

                            BrojIndeksa =
                                x.BrojIndeksa,

                            StudijskiProgram =
                                x.StudijskiProgram,

                            GodinaStudija =
                                x.GodinaStudija,

                            Prosek = x.Prosek,

                            BezRoditelja =
                                x.BezRoditelja,

                            DokumentacijaPrihodaDostavljena =
                                x.DokumentacijaPrihodaDostavljena,

                            UkupnaPrimanjaDomacinstva =
                                x.UkupnaPrimanjaDomacinstva,

                            UkupnoBodova =
                                x.UkupnoBodova,

                            DatumObjave =
                                x.DatumObjave
                        })
                    .ToList();

            return model;
        }

        private PreliminarnaRangListaServis
            KreirajServisRangListe()
        {
            IPreliminarnaRangListaRepository repo =
                new PreliminarnaRangListaRepositorySP(
                    _konekcija);

            return new PreliminarnaRangListaServis(
                repo);
        }

        private PrimedbaNaRangListuServis
            KreirajServisPrimedbi()
        {
            IPrimedbaNaRangListuRepository repo =
                new PrimedbaNaRangListuRepositorySP(
                    _konekcija);

            return new PrimedbaNaRangListuServis(
                repo);
        }
    }
}