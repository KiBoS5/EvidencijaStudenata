using KlasePodataka;

namespace PrezentacionaLogika
{
    /// <summary>
    /// Presentation layer for scoring configuration management
    /// </summary>
    public class FormaSkoringKonfiguracieKlasa
    {
        private string _konekcija;

        public FormaSkoringKonfiguracieKlasa(string konekcija)
        {
            _konekcija = konekcija;
        }

        /// <summary>
        /// Get current scoring configuration
        /// </summary>
        public SkoringKonfiguraciaKlasa DajKonfiguraciju()
        {
            var db = new SPSkoringKonfiguraciaDBKlasa(_konekcija);
            return db.DajKonfiguraciju();
        }

        /// <summary>
        /// Update scoring configuration (admin action)
        /// </summary>
        public bool AžurirajKonfiguraciju(SkoringKonfiguraciaKlasa konfiguracija)
        {
            var db = new SPSkoringKonfiguraciaDBKlasa(_konekcija);
            return db.AžurirajKonfiguraciju(konfiguracija);
        }
    }
}
