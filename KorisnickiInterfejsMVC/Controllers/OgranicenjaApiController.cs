using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Servisi;

namespace KorisnickiInterfejsMVC.Controllers
{
    public class OgranicenjaApiController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        [OutputCache(
            NoStore = true,
            Duration = 0,
            VaryByParam = "*")]
        public ActionResult Parametri()
        {
            try
            {
                var servis = new OgranicenjaServis();

                var ogranicenja = servis.DajOgranicenja();

                string json = JsonConvert.SerializeObject(
                    ogranicenja,
                    new JsonSerializerSettings
                    {
                        DateFormatHandling =
                            DateFormatHandling.IsoDateFormat
                    });

                return Content(
                    json,
                    "application/json",
                    System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(
                    ex.ToString());

                Response.StatusCode = 500;
                Response.TrySkipIisCustomErrors = true;

                return Content(
                    JsonConvert.SerializeObject(new
                    {
                        Poruka =
                            "Parametre konkursa trenutno nije moguće učitati."
                    }),
                    "application/json",
                    System.Text.Encoding.UTF8);
            }
        }
    }
}