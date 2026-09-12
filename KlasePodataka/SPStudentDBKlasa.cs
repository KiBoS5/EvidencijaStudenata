using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KlasePodataka.Mapiranja;

namespace KlasePodataka
{
    public class SPStudentDBKlasa
    {
        private string _stringKonekcije;

        public SPStudentDBKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        public List<StudentKlasa> DajSveStudente()
        {
            List<StudentKlasa> lista = new List<StudentKlasa>();

            using (SqlConnection konekcija = new SqlConnection(_stringKonekcije))
            {
                konekcija.Open();

                using (SqlCommand komanda = new SqlCommand("DajSveStudente", konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = komanda.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(StudentMapper.Mapiraj(reader));
                        }
                    }
                }
            }

            return lista;
        }

        public int DodajStudenta(
    StudentKlasa student,
    int maksimalanBrojPrijava)
        {
            if (student == null)
            {
                throw new ArgumentNullException("student");
            }

            if (maksimalanBrojPrijava <= 0)
            {
                throw new ArgumentException(
                    "Maksimalan broj prijava mora biti veći od nule.");
            }

            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand(
                    "dbo.DodajStudenta",
                    konekcija))
            {
                komanda.CommandType =
                    CommandType.StoredProcedure;

                komanda.Parameters.Add(
                    "@Ime",
                    SqlDbType.NVarChar,
                    100).Value =
                        student.Ime;

                komanda.Parameters.Add(
                    "@Prezime",
                    SqlDbType.NVarChar,
                    100).Value =
                        student.Prezime;

                komanda.Parameters.Add(
                    "@DatumRodjenja",
                    SqlDbType.Date).Value =
                        student.DatumRodjenja;

                komanda.Parameters.Add(
                    "@Email",
                    SqlDbType.NVarChar,
                    255).Value =
                        student.Email;

                komanda.Parameters.Add(
                    "@Telefon",
                    SqlDbType.NVarChar,
                    30).Value =
                        student.Telefon;

                komanda.Parameters.Add(
                    "@BrojIndeksa",
                    SqlDbType.NVarChar,
                    50).Value =
                        student.BrojIndeksa;

                komanda.Parameters.Add(
                    "@StudijskiProgram",
                    SqlDbType.NVarChar,
                    150).Value =
                        student.StudijskiProgram;

                komanda.Parameters.Add(
                    "@GodinaStudija",
                    SqlDbType.NVarChar,
                    50).Value =
                        student.GodinaStudija;

                komanda.Parameters.Add(
                    "@BezRoditelja",
                    SqlDbType.Bit).Value =
                        student.BezRoditelja;

                komanda.Parameters.Add(
                    "@Prosek",
                    SqlDbType.Decimal).Value =
                        student.Prosek;

                komanda.Parameters[
                    "@Prosek"].Precision = 5;

                komanda.Parameters[
                    "@Prosek"].Scale = 2;

                komanda.Parameters.Add(
                    "@DokumentacijaPrihodaDostavljena",
                    SqlDbType.Bit).Value =
                        student
                            .DokumentacijaPrihodaDostavljena;

                komanda.Parameters.Add(
                    "@UkupnaPrimanjaDomacinstva",
                    SqlDbType.Decimal).Value =
                        student
                            .UkupnaPrimanjaDomacinstva;

                komanda.Parameters[
                    "@UkupnaPrimanjaDomacinstva"]
                    .Precision = 15;

                komanda.Parameters[
                    "@UkupnaPrimanjaDomacinstva"]
                    .Scale = 2;

                komanda.Parameters.Add(
                    "@BrojClanovaPorodice",
                    SqlDbType.Int).Value =
                        student.BrojClanovaPorodice;

                komanda.Parameters.Add(
                    "@MaksimalanBrojPrijava",
                    SqlDbType.Int).Value =
                        maksimalanBrojPrijava;

                konekcija.Open();

                object rezultat =
                    komanda.ExecuteScalar();

                if (rezultat == null ||
                    rezultat == DBNull.Value)
                {
                    throw new InvalidOperationException(
                        "Procedura DodajStudenta nije vratila ID novog studenta.");
                }

                return Convert.ToInt32(
                    rezultat);
            }
        }
    
        
    }
}
