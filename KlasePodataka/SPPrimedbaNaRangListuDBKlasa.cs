using KlasePodataka.Mapiranja;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KlasePodataka
{
    public class SPPrimedbaNaRangListuDBKlasa
    {
        private readonly string _stringKonekcije;

        public SPPrimedbaNaRangListuDBKlasa(
            string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public void DodajPrimedbu(
            PrimedbaNaRangListuKlasa primedba)
        {
            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand(
                    "dbo.DodajPrimedbuNaRangListu",
                    konekcija))
            {
                komanda.CommandType =
                    CommandType.StoredProcedure;

                komanda.Parameters.Add(
                    "@Ime",
                    SqlDbType.NVarChar,
                    100).Value =
                        primedba.Ime;

                komanda.Parameters.Add(
                    "@Prezime",
                    SqlDbType.NVarChar,
                    100).Value =
                        primedba.Prezime;

                komanda.Parameters.Add(
                    "@Email",
                    SqlDbType.NVarChar,
                    255).Value =
                        primedba.Email;

                komanda.Parameters.Add(
                    "@Komentar",
                    SqlDbType.NVarChar,
                    2000).Value =
                        primedba.Komentar;

                konekcija.Open();
                komanda.ExecuteNonQuery();
            }
        }

        public List<PrimedbaNaRangListuKlasa>
            DajPrimedbe()
        {
            List<PrimedbaNaRangListuKlasa> lista =
                new List<PrimedbaNaRangListuKlasa>();

            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand(
                    "dbo.DajPrimedbeNaRangListu",
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
                        lista.Add(PrimedbaNaRangListuMapper.Mapiraj(reader));
                    }
                }
            }

            return lista;
        }

        public void OznaciObradjenom(
            int primedbaID,
            int obradioKorisnikID)
        {
            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand(
                    "dbo.OznaciPrimedbuObradjenom",
                    konekcija))
            {
                komanda.CommandType =
                    CommandType.StoredProcedure;

                komanda.Parameters.Add(
                    "@PrimedbaID",
                    SqlDbType.Int).Value =
                        primedbaID;

                komanda.Parameters.Add(
                    "@ObradioKorisnikID",
                    SqlDbType.Int).Value =
                        obradioKorisnikID;

                konekcija.Open();
                komanda.ExecuteNonQuery();
            }
        }

        
        }
    }
