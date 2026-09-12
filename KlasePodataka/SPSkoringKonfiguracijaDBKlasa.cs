using System;
using System.Data;
using System.Data.SqlClient;
using KlasePodataka.Mapiranja;

namespace KlasePodataka
{
    public class SPSkoringKonfiguracijaDBKlasa
    {
        private readonly string _stringKonekcije;

        public SPSkoringKonfiguracijaDBKlasa(string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public SkoringKonfiguracijaKlasa DajKonfiguraciju()
        {
            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand(
                    "dbo.DajSkoringKonfiguraciju",
                    konekcija))
            {
                komanda.CommandType = CommandType.StoredProcedure;

                konekcija.Open();

                using (SqlDataReader reader = komanda.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException(
                            "Konfiguracija bodovanja nije pronađena.");
                    }

                    return SkoringKonfiguracijaMapper.Mapiraj(reader);
                }
            }
        }

        public void SacuvajKonfiguraciju(
            SkoringKonfiguracijaKlasa konfiguracija)
        {
            if (konfiguracija == null)
            {
                throw new ArgumentNullException(nameof(konfiguracija));
            }

            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand(
                    "dbo.SacuvajSkoringKonfiguraciju",
                    konekcija))
            {
                komanda.CommandType = CommandType.StoredProcedure;

                komanda.Parameters.Add(
                    "@ID",
                    SqlDbType.Int).Value = konfiguracija.ID;

                DodajDecimalParametar(
                    komanda,
                    "@MinimalanProsek",
                    konfiguracija.MinimalanProsek,
                    5,
                    2);

                DodajDecimalParametar(
                    komanda,
                    "@ProsekZa10Bodova",
                    konfiguracija.ProsekZa10Bodova,
                    5,
                    2);

                DodajDecimalParametar(
                    komanda,
                    "@ProsekZa15Bodova",
                    konfiguracija.ProsekZa15Bodova,
                    5,
                    2);

                DodajDecimalParametar(
                    komanda,
                    "@ProsekZa20Bodova",
                    konfiguracija.ProsekZa20Bodova,
                    5,
                    2);

                DodajDecimalParametar(
                    komanda,
                    "@PrimanjaZa10Bodova",
                    konfiguracija.PrimanjaZa10Bodova,
                    15,
                    2);

                DodajDecimalParametar(
                    komanda,
                    "@PrimanjaZa1Bod",
                    konfiguracija.PrimanjaZa1Bod,
                    15,
                    2);

                komanda.Parameters.Add(
                    "@BodoviGodinaStudije1",
                    SqlDbType.Int).Value =
                        konfiguracija.BodoviGodinaStudije1;

                komanda.Parameters.Add(
                    "@BodoviGodinaStudije2",
                    SqlDbType.Int).Value =
                        konfiguracija.BodoviGodinaStudije2;

                komanda.Parameters.Add(
                    "@BodoviGodinaStudije3",
                    SqlDbType.Int).Value =
                        konfiguracija.BodoviGodinaStudije3;

                komanda.Parameters.Add(
                    "@BodoviGodinaStudije4",
                    SqlDbType.Int).Value =
                        konfiguracija.BodoviGodinaStudije4;

                komanda.Parameters.Add(
                    "@BodoviMaster",
                    SqlDbType.Int).Value =
                        konfiguracija.BodoviMaster;

                DodajDecimalParametar(
                    komanda,
                    "@BezRoditeljaMultiplier",
                    konfiguracija.BezRoditeljaMultiplier,
                    3,
                    2);

                konekcija.Open();
                komanda.ExecuteNonQuery();
            }
        }

        private static void DodajDecimalParametar(
            SqlCommand komanda,
            string naziv,
            decimal vrednost,
            byte precision,
            byte scale)
        {
            SqlParameter parametar =
                komanda.Parameters.Add(naziv, SqlDbType.Decimal);

            parametar.Precision = precision;
            parametar.Scale = scale;
            parametar.Value = vrednost;
        }
    }
}