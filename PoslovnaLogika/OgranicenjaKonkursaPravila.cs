using System;
using Servisi;

namespace PoslovnaLogika
{
    public class OgranicenjaKonkursaPravila
    {
        public void ProveriDodavanjePrijave(
            OgranicenjaKonkursaKlasa ogranicenja,
            DateTimeOffset sada)
        {
            ProveriParametre(ogranicenja);

            if (sada < ogranicenja.DatumPocetkaKonkursa)
            {
                throw new ArgumentException(
                    "Konkurs još nije otvoren.");
            }

            if (sada > ogranicenja.DatumZavrsetkaKonkursa)
            {
                throw new ArgumentException(
                    "Konkurs je završen. Unos prijava nije dozvoljen.");
            }
        }

        public void ProveriBrojPrijava(
            OgranicenjaKonkursaKlasa ogranicenja,
            int trenutniBrojPrijava)
        {
            ProveriParametre(ogranicenja);

            if (trenutniBrojPrijava < 0)
            {
                throw new ArgumentException(
                    "Broj postojećih prijava nije ispravan.");
            }

            if (trenutniBrojPrijava >=
                ogranicenja.MaksimalanBrojPrijava)
            {
                throw new ArgumentException(
                    "Dostignut je maksimalan broj prijava.");
            }
        }

        public void ProveriPodnosenjePrimedbe(
            OgranicenjaKonkursaKlasa ogranicenja,
            bool listaJeObjavljena,
            DateTimeOffset sada)
        {
            ProveriParametre(ogranicenja);

            if (!listaJeObjavljena)
            {
                throw new ArgumentException(
                    "Primedbe su dozvoljene tek nakon objavljivanja " +
                    "preliminarne rang-liste.");
            }

            if (sada > ogranicenja.RokZaPrimedbe)
            {
                throw new ArgumentException(
                    "Rok za podnošenje primedbi je istekao.");
            }
        }

        private static void ProveriParametre(
            OgranicenjaKonkursaKlasa ogranicenja)
        {
            if (ogranicenja == null)
            {
                throw new InvalidOperationException(
                    "Parametri konkursa nisu dostupni.");
            }

            if (ogranicenja.DatumPocetkaKonkursa ==
                default(DateTimeOffset))
            {
                throw new InvalidOperationException(
                    "Datum početka konkursa nije podešen.");
            }

            if (ogranicenja.DatumZavrsetkaKonkursa <=
                ogranicenja.DatumPocetkaKonkursa)
            {
                throw new InvalidOperationException(
                    "Datum završetka mora biti posle datuma početka.");
            }

            if (ogranicenja.MaksimalanBrojPrijava <= 0)
            {
                throw new InvalidOperationException(
                    "Maksimalan broj prijava mora biti veći od nule.");
            }

            if (ogranicenja.RokZaPrimedbe <=
                ogranicenja.DatumZavrsetkaKonkursa)
            {
                throw new InvalidOperationException(
                    "Rok za primedbe mora biti posle završetka konkursa.");
            }
        }
    }
}