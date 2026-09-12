using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KlasePodataka.Mapiranja;

namespace KlasePodataka
{
    public class SPStudentDokumentacijaDBKlasa
    {
        private readonly string _stringKonekcije;

        public SPStudentDokumentacijaDBKlasa(
            string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public List<StudentDokumentacijaKlasa>
            DajDokumentacijuStudenata()
        {
            List<StudentDokumentacijaKlasa> lista =
                new List<StudentDokumentacijaKlasa>();

            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand(
                    "dbo.DajDokumentacijuStudenata",
                    konekcija))
            {
                komanda.CommandType =
                    CommandType.StoredProcedure;

                konekcija.Open();

                using (SqlDataReader reader =
                    komanda.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(
                                StudentDokumentacijaMapper.Mapiraj(reader));
                    }
                }
            }

            return lista;
        }

        public void OznaciDokumentacijuPodnetom(
            int studentID)
        {
            IzvrsiZaStudenta(
                "dbo.OznaciDokumentacijuPodnetom",
                studentID);
        }

        public void ProveriDokumentacijuStudenta(
            int studentID,
            bool dokumentacijaIspravna,
            string napomena,
            int proverioKorisnikID)
        {
            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand(
                    "dbo.ProveriDokumentacijuStudenta",
                    konekcija))
            {
                komanda.CommandType =
                    CommandType.StoredProcedure;

                komanda.Parameters.Add(
                    "@StudentID",
                    SqlDbType.Int).Value =
                        studentID;

                komanda.Parameters.Add(
                    "@DokumentacijaIspravna",
                    SqlDbType.Bit).Value =
                        dokumentacijaIspravna;

                komanda.Parameters.Add(
                    "@Napomena",
                    SqlDbType.NVarChar,
                    500).Value =
                        string.IsNullOrWhiteSpace(napomena)
                            ? (object)DBNull.Value
                            : napomena.Trim();

                komanda.Parameters.Add(
                    "@ProverioKorisnikID",
                    SqlDbType.Int).Value =
                        proverioKorisnikID;

                konekcija.Open();
                komanda.ExecuteNonQuery();
            }
        }

        private void IzvrsiZaStudenta(
            string nazivProcedure,
            int studentID)
        {
            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand(
                    nazivProcedure,
                    konekcija))
            {
                komanda.CommandType =
                    CommandType.StoredProcedure;

                komanda.Parameters.Add(
                    "@StudentID",
                    SqlDbType.Int).Value =
                        studentID;

                konekcija.Open();
                komanda.ExecuteNonQuery();
            }
        }

        
        
    }
}