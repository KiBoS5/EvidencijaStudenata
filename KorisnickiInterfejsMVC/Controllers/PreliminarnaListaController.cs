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
        public ActionResult PodnesiPrimedbu(
            [Bind(Prefix = "Primedba")]
            PrimedbaNaRangListuVM primedba)
        {
            PreliminarnaRangListaJavniVM model =
                KreirajJavniModel();

            model.Primedba =
                primedba ??
                new PrimedbaNaRangListuVM();

            if (!model.ListaJeObjavljena)
            {
                ModelState.AddModelError(
                    "",
                    "Preliminarna rang-lista nije objavljena.");

                return View(
                    "Index",
                    model);
            }

            if (!ModelState.IsValid)
            {
                return View(
                    "Index",
                    model);
            }

            try
            {
                PrimedbaNaRangListuKlasa novaPrimedba =
                    new PrimedbaNaRangListuKlasa
                    {
                        Ime = primedba.Ime,
                        Prezime = primedba.Prezime,
                        Email = primedba.Email,
                        Komentar = primedba.Komentar
                    };

                KreirajServisPrimedbi()
                    .Dodaj(novaPrimedba);

                TempData["Poruka"] =
                    "Primedba je uspešno podneta.";

                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);
            }
            catch (SqlException ex)
                when (ex.Number >= 50030 &&
                      ex.Number <= 50032)
            {
                ModelState.AddModelError(
                    "",
                    ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "Dogodila se greška pri podnošenju primedbe.");
            }

            return View(
                "Index",
                model);
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