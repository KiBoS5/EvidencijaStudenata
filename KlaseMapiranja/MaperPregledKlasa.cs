using System;
using System.Data.SqlClient;
using KlasePodataka;

namespace KlaseMapiranja
{
    public static class StudentMapper
    {
        public static StudentKlasa Mapiraj(SqlDataReader reader)
        {
            return new StudentKlasa
            {
                ID = Convert.ToInt32(reader["ID"]),
                Ime = reader["Ime"].ToString(),
                Prezime = reader["Prezime"].ToString(),
                Email = reader["Email"].ToString(),
                BrojIndeksa = reader["BrojIndeksa"].ToString(),
                Prosek = Convert.ToDecimal(reader["Prosek"]),
                UkupnoBodova = Convert.ToInt32(reader["UkupnoBodova"])
            };
        }
    }
}