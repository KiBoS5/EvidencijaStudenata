using KlasePodataka;
using System;
using System.Data.SqlClient;

namespace Repozitorijumi.Mapiranja
{
    internal static class PreliminarnaRangListaMapper
    {
        public static PreliminarnaRangListaKlasa Mapiraj(
            SqlDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return new PreliminarnaRangListaKlasa
            {
                ID =
                    Convert.ToInt32(reader["ID"]),

                Pozicija =
                    Convert.ToInt32(reader["Pozicija"]),

                StudentID =
                    Convert.ToInt32(reader["StudentID"]),

                Ime =
                    ProcitajString(reader, "Ime"),

                Prezime =
                    ProcitajString(reader, "Prezime"),

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

                Prosek =
                    Convert.ToDecimal(reader["Prosek"]),

                BezRoditelja =
                    Convert.ToBoolean(
                        reader["BezRoditelja"]),

                DokumentacijaPrihodaDostavljena =
                    Convert.ToBoolean(
                        reader[
                            "DokumentacijaPrihodaDostavljena"]),

                UkupnaPrimanjaDomacinstva =
                    Convert.ToDecimal(
                        reader[
                            "UkupnaPrimanjaDomacinstva"]),

                UkupnoBodova =
                    Convert.ToInt32(
                        reader["UkupnoBodova"]),

                DatumObjave =
                    Convert.ToDateTime(
                        reader["DatumObjave"])
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
    }
}