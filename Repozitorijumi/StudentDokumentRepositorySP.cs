using KlasePodataka;
using Repozitorijumi.Mapiranja;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Repozitorijumi
{
    public class StudentDokumentRepositorySP : IStudentDokumentRepository
    {
        private readonly string _stringKonekcije;

        public StudentDokumentRepositorySP(string stringKonekcije)
        {
            if (string.IsNullOrWhiteSpace(stringKonekcije))
            {
                throw new ArgumentException(
                    "String konekcije nije podešen.",
                    nameof(stringKonekcije));
            }

            _stringKonekcije = stringKonekcije;
        }

        public List<StudentDokumentKlasa> DajDokumenteStudenta(
            int studentID)
        {
            var dokumenti = new List<StudentDokumentKlasa>();

            using (var konekcija =
                new SqlConnection(_stringKonekcije))
            using (var komanda =
                new SqlCommand(
                    "dbo.DajDokumenteStudenta",
                    konekcija))
            {
                komanda.CommandType = CommandType.StoredProcedure;

                komanda.Parameters.Add(
                    "@StudentID",
                    SqlDbType.Int).Value = studentID;

                konekcija.Open();

                using (SqlDataReader reader = komanda.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dokumenti.Add(
                            StudentDokumentMapper.Mapiraj(reader));
                    }
                }
            }

            return dokumenti;
        }

        public void OznaciPodnetim(
    int studentID,
    int dokumentID)
        {
            using (var konekcija =
                new SqlConnection(_stringKonekcije))
            using (var komanda =
                new SqlCommand(
                    "dbo.OznaciStudentDokumentPodnetim",
                    konekcija))
            {
                komanda.CommandType = CommandType.StoredProcedure;

                komanda.Parameters.Add(
                    "@StudentID",
                    SqlDbType.Int).Value = studentID;

                komanda.Parameters.Add(
                    "@DokumentID",
                    SqlDbType.Int).Value = dokumentID;

                konekcija.Open();
                komanda.ExecuteNonQuery();
            }
        }

        public void ProveriDokument(
            int studentID,
            int dokumentID,
            bool ispravan,
            int proverioKorisnikID,
            string napomena)
        {
            using (var konekcija =
                new SqlConnection(_stringKonekcije))
            using (var komanda =
                new SqlCommand(
                    "dbo.ProveriStudentDokument",
                    konekcija))
            {
                komanda.CommandType = CommandType.StoredProcedure;

                komanda.Parameters.Add(
                    "@StudentID",
                    SqlDbType.Int).Value = studentID;

                komanda.Parameters.Add(
                    "@DokumentID",
                    SqlDbType.Int).Value = dokumentID;

                komanda.Parameters.Add(
                    "@Ispravan",
                    SqlDbType.Bit).Value = ispravan;

                komanda.Parameters.Add(
                    "@ProverioKorisnikID",
                    SqlDbType.Int).Value = proverioKorisnikID;

                komanda.Parameters.Add(
                    "@Napomena",
                    SqlDbType.NVarChar,
                    500).Value =
                        string.IsNullOrWhiteSpace(napomena)
                            ? (object)DBNull.Value
                            : napomena.Trim();

                konekcija.Open();
                komanda.ExecuteNonQuery();
            }
        }
    }
}