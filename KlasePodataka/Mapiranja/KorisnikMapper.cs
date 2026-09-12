using System;
using System.Data.SqlClient;

namespace KlasePodataka.Mapiranja
{
    internal static class KorisnikMapper
    {
        public static KorisnikKlasa Mapiraj(
            SqlDataReader reader,
            bool ukljuciLozinkaHash)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return new KorisnikKlasa
            {
                ID =
                    Convert.ToInt32(reader["ID"]),

                Ime =
                    ProcitajString(reader, "Ime"),

                Prezime =
                    ProcitajString(reader, "Prezime"),

                KorisnickoIme =
                    ProcitajString(
                        reader,
                        "KorisnickoIme"),

                LozinkaHash =
                    ukljuciLozinkaHash
                        ? ProcitajString(
                            reader,
                            "LozinkaHash")
                        : null,

                TipKorisnika =
                    (TipKorisnika)Convert.ToByte(
                        reader["TipKorisnika"]),

                Aktivan =
                    Convert.ToBoolean(
                        reader["Aktivan"]),

                DatumKreiranja =
                    Convert.ToDateTime(
                        reader["DatumKreiranja"]),

                DatumPoslednjePrijave =
                    ProcitajNullableDatum(
                        reader,
                        "DatumPoslednjePrijave")
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

        private static DateTime? ProcitajNullableDatum(
            SqlDataReader reader,
            string kolona)
        {
            return reader[kolona] == DBNull.Value
                ? (DateTime?)null
                : Convert.ToDateTime(
                    reader[kolona]);
        }
    }
}