using System;
using System.Web.Mvc;
using KlasePodataka;
using System.Configuration;
using System.Data.SqlClient;
using KorisnickiInterfejsMVC.Models;
using Repozitorijumi;
using Servisi;

namespace KorisnickiInterfejsMVC.Controllers
{
    public class ZaposleniController : Controller
    {
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

            if (tipKorisnika !=
                (byte)TipKorisnika.Zaposleni)
            {
                filterContext.Result =
                    RedirectToAction(
                        "Index",
                        "Pocetna");
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}