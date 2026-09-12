using System;
using System.Data;
using System.Data.SqlClient;

namespace KlasePodataka
{
    public class SPStudentDokumentaciaDBKlasa
    {
        private string _stringKonekcije;

        public SPStudentDokumentaciaDBKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // =========================
        // GET DOCUMENT STATUS FOR STUDENT
        // =========================
        public StudentDokumentaciaKlasa DajDokumentacijuStudenta(int studentID)
        {
            StudentDokumentaciaKlasa dokumentacija = null;

            using (SqlConnection konekcija = new SqlConnection(_stringKonekcije))
            {
                konekcija.Open();

                using (SqlCommand komanda = new SqlCommand("DajDokumentacijuStudenta", konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;
                    komanda.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentID;

                    using (SqlDataReader reader = komanda.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dokumentacija = MapirajDokumentaciju(reader);
                        }
                    }
                }
            }

            return dokumentacija;
        }

        // =========================
        // CREATE DOCUMENT RECORD
        // =========================
        public int KreirajDokumentaciju(StudentDokumentaciaKlasa dokumentacija)
        {
            int noviID = 0;

            using (SqlConnection konekcija = new SqlConnection(_stringKonekcije))
            {
                konekcija.Open();

                using (SqlCommand komanda = new SqlCommand("KreirajStudentDokumentaciju", konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;

                    komanda.Parameters.Add("@StudentID", SqlDbType.Int).Value = dokumentacija.StudentID;
                    komanda.Parameters.Add("@SviDokumentiPodneseni", SqlDbType.Bit).Value = dokumentacija.SviDokumentiPodneseni;
                    komanda.Parameters.Add("@DatumPodnosenja", SqlDbType.DateTime).Value = dokumentacija.DatumPodnosenja ?? (object)DBNull.Value;

                    object rezultat = komanda.ExecuteScalar();
                    if (rezultat != null)
                    {
                        noviID = Convert.ToInt32(rezultat);
                    }
                }
            }

            return noviID;
        }

        // =========================
        // UPDATE DOCUMENT STATUS
        // =========================
        public bool AžurirajDokumentaciju(StudentDokumentaciaKlasa dokumentacija)
        {
            int rezultat = 0;

            using (SqlConnection konekcija = new SqlConnection(_stringKonekcije))
            {
                konekcija.Open();

                using (SqlCommand komanda = new SqlCommand("AžurirajStudentDokumentaciju", konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;

                    komanda.Parameters.Add("@ID", SqlDbType.Int).Value = dokumentacija.ID;
                    komanda.Parameters.Add("@SviDokumentiPodneseni", SqlDbType.Bit).Value = dokumentacija.SviDokumentiPodneseni;
                    komanda.Parameters.Add("@DatumPodnosenja", SqlDbType.DateTime).Value = dokumentacija.DatumPodnosenja ?? (object)DBNull.Value;
                    komanda.Parameters.Add("@SviDokumentiProvereni", SqlDbType.Bit).Value = dokumentacija.SviDokumentiProvereni;
                    komanda.Parameters.Add("@DatumProvere", SqlDbType.DateTime).Value = dokumentacija.DatumProvere ?? (object)DBNull.Value;
                    komanda.Parameters.Add("@NapomenaProvere", SqlDbType.NVarChar).Value = dokumentacija.NapomenaProvere ?? (object)DBNull.Value;
                    komanda.Parameters.Add("@SpremnaZaUnos", SqlDbType.Bit).Value = dokumentacija.SpremnaZaUnos;

                    rezultat = komanda.ExecuteNonQuery();
                }
            }

            return (rezultat > 0);
        }

        // =========================
        // VERIFY ALL DOCUMENTS FOR STUDENT
        // =========================
        public bool ProveridokumentacijuStudenta(int studentID, string napomena)
        {
            int rezultat = 0;

            using (SqlConnection konekcija = new SqlConnection(_stringKonekcije))
            {
                konekcija.Open();

                using (SqlCommand komanda = new SqlCommand("ProveridokumentacijuStudenta", konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;

                    komanda.Parameters.Add("@StudentID", SqlDbType.Int).Value = studentID;
                    komanda.Parameters.Add("@DatumProvere", SqlDbType.DateTime).Value = DateTime.Now;
                    komanda.Parameters.Add("@NapomenaProvere", SqlDbType.NVarChar).Value = napomena ?? (object)DBNull.Value;

                    rezultat = komanda.ExecuteNonQuery();
                }
            }

            return (rezultat > 0);
        }

        private StudentDokumentaciaKlasa MapirajDokumentaciju(SqlDataReader reader)
        {
            return new StudentDokumentaciaKlasa
            {
                ID = Convert.ToInt32(reader["ID"]),
                StudentID = Convert.ToInt32(reader["StudentID"]),
                SviDokumentiPodneseni = Convert.ToBoolean(reader["SviDokumentiPodneseni"]),
                DatumPodnosenja = reader["DatumPodnosenja"] != DBNull.Value ? Convert.ToDateTime(reader["DatumPodnosenja"]) : (DateTime?)null,
                SviDokumentiProvereni = Convert.ToBoolean(reader["SviDokumentiProvereni"]),
                DatumProvere = reader["DatumProvere"] != DBNull.Value ? Convert.ToDateTime(reader["DatumProvere"]) : (DateTime?)null,
                NapomenaProvere = reader["NapomenaProvere"]?.ToString(),
                SpremnaZaUnos = Convert.ToBoolean(reader["SpremnaZaUnos"]),
                DatumKreiranja = Convert.ToDateTime(reader["DatumKreiranja"]),
                DatumAžuriranja = Convert.ToDateTime(reader["DatumAžuriranja"])
            };
        }
    }
}
