using KlasePodataka;
using System;
using System.Data.SqlClient;

namespace Repozitorijumi.Mapiranja
{
    internal static class StudentMapper
    {
        public static StudentKlasa Mapiraj(
            SqlDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return new StudentKlasa
            {
                ID =
                    Convert.ToInt32(reader["ID"]),

                Ime =
                    ProcitajString(reader, "Ime"),

                Prezime =
                    ProcitajString(reader, "Prezime"),

                DatumRodjenja =
                    Convert.ToDateTime(
                        reader["DatumRodjenja"]),

                Email =
                    ProcitajString(reader, "Email"),

                Telefon =
                    ProcitajString(reader, "Telefon"),

                BrojIndeksa =
                    ProcitajString(reader, "BrojIndeksa"),

                StudijskiProgram =
                    ProcitajString(
                        reader,
                        "StudijskiProgram"),

                GodinaStudija =
                    ProcitajString(
                        reader,
                        "GodinaStudija"),

                BezRoditelja =
                    Convert.ToBoolean(
                        reader["BezRoditelja"]),

                Prosek =
                    Convert.ToDecimal(
                        reader["Prosek"]),

                DokumentacijaPrihodaDostavljena =
                    Convert.ToBoolean(
                        reader[
                            "DokumentacijaPrihodaDostavljena"]),

                UkupnaPrimanjaDomacinstva =
                    Convert.ToDecimal(
                        reader[
                            "UkupnaPrimanjaDomacinstva"]),

                BrojClanovaPorodice =
                    Convert.ToInt32(
                        reader["BrojClanovaPorodice"]),

                BodoviProsek =
                ProcitajInt(reader,"BodoviProsek"),

                BodoviPrimanja =
                ProcitajInt(reader,"BodoviPrimanja"),

                BodoviGodina =
                ProcitajInt(reader,"BodoviGodina"),

                UkupnoBodova =
                ProcitajInt(reader,"UkupnoBodova"),

                DokumentacijaSpremnaZaRangiranje =
                    Convert.ToBoolean(
                        reader["DokumentacijaSpremnaZaRangiranje"])
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

        private static int ProcitajInt(
            SqlDataReader reader,
            string kolona)
        {
            return reader[kolona] == DBNull.Value
                ? 0
                : Convert.ToInt32(
                    reader[kolona]);
        }
    }
}