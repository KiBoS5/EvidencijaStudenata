using KlasePodataka;
using System;
using System.Data.SqlClient;

namespace Repozitorijumi.Mapiranja
{
    internal static class StudentDokumentacijaMapper
    {
        public static StudentDokumentacijaKlasa Mapiraj(
            SqlDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return new StudentDokumentacijaKlasa
            {
                ID = Convert.ToInt32(reader["ID"]),
                StudentID = Convert.ToInt32(reader["StudentID"]),

                SviDokumentiPodneseni =
                    Convert.ToBoolean(
                        reader["SviDokumentiPodneseni"]),

                DatumPodnosenja =
                    ProcitajNullableDatum(
                        reader,
                        "DatumPodnosenja"),

                SviDokumentiProvereni =
                    Convert.ToBoolean(
                        reader["SviDokumentiProvereni"]),

                DatumProvere =
                    ProcitajNullableDatum(
                        reader,
                        "DatumProvere"),

                DokumentacijaIspravna =
                    ProcitajNullableBool(
                        reader,
                        "DokumentacijaIspravna"),

                NapomenaProvere =
                    ProcitajString(
                        reader,
                        "NapomenaProvere"),

                SpremnaZaUnos =
                    Convert.ToBoolean(
                        reader["SpremnaZaUnos"]),

                DatumKreiranja =
                    Convert.ToDateTime(
                        reader["DatumKreiranja"]),

                DatumAzuriranja =
                    Convert.ToDateTime(
                        reader["DatumAzuriranja"]),

                ProverioKorisnikID =
                    ProcitajNullableInt(
                        reader,
                        "ProverioKorisnikID"),

                ProverioIme =
                    ProcitajString(
                        reader,
                        "ProverioIme"),

                ProverioPrezime =
                    ProcitajString(
                        reader,
                        "ProverioPrezime"),

                ProverioKorisnickoIme =
                    ProcitajString(
                        reader,
                        "ProverioKorisnickoIme"),

                StudentIme =
                    ProcitajString(
                        reader,
                        "StudentIme"),

                StudentPrezime =
                    ProcitajString(
                        reader,
                        "StudentPrezime"),

                BrojIndeksa =
                    ProcitajString(
                        reader,
                        "BrojIndeksa")
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

        private static bool? ProcitajNullableBool(
            SqlDataReader reader,
            string kolona)
        {
            return reader[kolona] == DBNull.Value
                ? (bool?)null
                : Convert.ToBoolean(reader[kolona]);
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