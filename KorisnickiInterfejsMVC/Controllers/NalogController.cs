using System;
using System.Configuration;
using System.Web.Mvc;
using KlasePodataka;
using KorisnickiInterfejsMVC.Models;
using PoslovnaLogika;
using Repozitorijumi;
namespace KorisnickiInterfejsMVC.Controllers
{
    public class NalogController : Controller
    {
        private readonly string _konekcija =
            ConfigurationManager
                .ConnectionStrings["Konekcija"]
                .ConnectionString;

        [HttpGet]
        public ActionResult UlogujAdmin()
        {
            if (Session["KorisnikID"] != null &&
                Session["TipKorisnika"] != null)
            {
                TipKorisnika tip =
                    (TipKorisnika)Convert.ToByte(
                        Session["TipKorisnika"]);

                return PreusmeriKorisnika(tip);
            }

            return View(new UlogujVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UlogujAdmin(UlogujVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                IKorisnikRepository repo =
                    new KorisnikRepositorySP(_konekcija);

                KorisnikServis servis =
                    new KorisnikServis(repo);

                KorisnikKlasa korisnik =
                    servis.PrijaviKorisnika(
                        model.KorisnickoIme,
                        model.Lozinka);

                if (korisnik == null)
                {
                    ModelState.AddModelError(
                        "",
                        "Pogrešno korisničko ime ili lozinka.");

                    return View(model);
                }

                Session.Clear();

                Session["KorisnikID"] = korisnik.ID;
                Session["KorisnickoIme"] =
                    korisnik.KorisnickoIme;
                Session["ImeIPrezime"] =
                    korisnik.ImeIPrezime;
                Session["TipKorisnika"] =
                    (byte)korisnik.TipKorisnika;

                return PreusmeriKorisnika(
                    korisnik.TipKorisnika);
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "Prijava trenutno nije dostupna. Pokušajte ponovo.");

                return View(model);
            }
        }

        public ActionResult Izloguj()
        {
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("UlogujAdmin");
        }

        private ActionResult PreusmeriKorisnika(
            TipKorisnika tipKorisnika)
        {
            if (tipKorisnika ==
                TipKorisnika.Administrator)
            {
                return RedirectToAction(
                    "Index",
                    "Admin");
            }

            return RedirectToAction(
                "Index",
                "Zaposleni");
        }
    }
}