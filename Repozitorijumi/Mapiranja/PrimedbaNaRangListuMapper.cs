using KlasePodataka;
using System;
using System.Data.SqlClient;

namespace Repozitorijumi.Mapiranja
{
    internal static class PrimedbaNaRangListuMapper
    {
        public static PrimedbaNaRangListuKlasa Mapiraj(
            SqlDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return new PrimedbaNaRangListuKlasa
            {
                ID =
                    Convert.ToInt32(reader["ID"]),

                Ime =
                    ProcitajString(reader, "Ime"),

                Prezime =
                    ProcitajString(reader, "Prezime"),

                Email =
                    ProcitajString(reader, "Email"),

                Komentar =
                    ProcitajString(reader, "Komentar"),

                DatumPodnosenja =
                    Convert.ToDateTime(
                        reader["DatumPodnosenja"]),

                Obradjena =
                    Convert.ToBoolean(
                        reader["Obradjena"]),

                DatumObrade =
                    ProcitajNullableDatum(
                        reader,
                        "DatumObrade"),

                ObradioKorisnikID =
                    ProcitajNullableInt(
                        reader,
                        "ObradioKorisnikID"),

                ObradioIme =
                    ProcitajString(
                        reader,
                        "ObradioIme"),

                ObradioPrezime =
                    ProcitajString(
                        reader,
                        "ObradioPrezime"),

                ObradioKorisnickoIme =
                    ProcitajString(
                        reader,
                        "ObradioKorisnickoIme")
            };
        }

        private static string ProcitajString(
            SqlDataReader reader,
            string kolona)
        {
            return reader[kolona] == DBNull.Value
                ? null
                : reader[kolona].ToString();
        }

        private static int? ProcitajNullableInt(
            SqlDataReader reader,
            string kolona)
        {
            return reader[kolona] == DBNull.Value
                ? (int?)null
                : Convert.ToInt32(reader[kolona]);
        }

        private static DateTime? ProcitajNullableDatum(
            SqlDataReader reader,
            string kolona)
        {
            return reader[kolona] == DBNull.Value
                ? (DateTime?)null
                : Convert.ToDateTime(reader[kolona]);
        }
    }
}