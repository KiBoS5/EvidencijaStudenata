using System;
using System.Data.SqlClient;

namespace KlasePodataka.Mapiranja
{
    internal static class StudentDokumentMapper
    {
        public static StudentDokumentKlasa Mapiraj(
            SqlDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return new StudentDokumentKlasa
            {
                ID = Convert.ToInt32(reader["ID"]),

                StudentID =
                    Convert.ToInt32(reader["StudentID"]),

                VrstaDokumentaID =
                    Convert.ToInt32(reader["VrstaDokumentaID"]),

                NazivDokumenta =
                    reader["NazivDokumenta"].ToString(),

                Obavezan =
                    Convert.ToBoolean(reader["Obavezan"]),

                Podnet =
                    Convert.ToBoolean(reader["Podnet"]),

                DatumPodnosenja =
                    reader["DatumPodnosenja"] == DBNull.Value
                        ? (DateTime?)null
                        : Convert.ToDateTime(reader["DatumPodnosenja"]),

                Ispravan =
                    reader["Ispravan"] == DBNull.Value
                        ? (bool?)null
                        : Convert.ToBoolean(reader["Ispravan"]),

                DatumProvere =
                    reader["DatumProvere"] == DBNull.Value
                        ? (DateTime?)null
                        : Convert.ToDateTime(reader["DatumProvere"]),

                ProverioKorisnikID =
                    reader["ProverioKorisnikID"] == DBNull.Value
                        ? (int?)null
                        : Convert.ToInt32(reader["ProverioKorisnikID"]),

                ProverioImeIPrezime =
                    reader["ProverioImeIPrezime"] == DBNull.Value
                        ? null
                        : reader["ProverioImeIPrezime"].ToString(),

                Napomena =
                    reader["Napomena"] == DBNull.Value
                        ? null
                        : reader["Napomena"].ToString(),

                DatumKreiranja =
                    Convert.ToDateTime(reader["DatumKreiranja"]),

                DatumAzuriranja =
                    Convert.ToDateTime(reader["DatumAzuriranja"])
            };
        }
    }
}