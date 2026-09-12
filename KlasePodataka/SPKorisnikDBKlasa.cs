using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KlasePodataka.Mapiranja;

namespace KlasePodataka
{
    public class SPKorisnikDBKlasa
    {
        private readonly string _stringKonekcije;

        public SPKorisnikDBKlasa(string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public int DodajKorisnika(KorisnikKlasa korisnik)
        {
            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand("dbo.DodajKorisnika", konekcija))
            {
                komanda.CommandType = CommandType.StoredProcedure;

                komanda.Parameters.Add(
                    "@Ime",
                    SqlDbType.NVarChar,
                    100).Value = korisnik.Ime;

                komanda.Parameters.Add(
                    "@Prezime",
                    SqlDbType.NVarChar,
                    100).Value = korisnik.Prezime;

                komanda.Parameters.Add(
                    "@KorisnickoIme",
                    SqlDbType.NVarChar,
                    50).Value = korisnik.KorisnickoIme;

                komanda.Parameters.Add(
                    "@LozinkaHash",
                    SqlDbType.NVarChar,
                    255).Value = korisnik.LozinkaHash;

                komanda.Parameters.Add(
                    "@TipKorisnika",
                    SqlDbType.TinyInt).Value =
                        (byte)korisnik.TipKorisnika;

                komanda.Parameters.Add(
                    "@Aktivan",
                    SqlDbType.Bit).Value = korisnik.Aktivan;

                konekcija.Open();

                object rezultat = komanda.ExecuteScalar();

                if (rezultat == null || rezultat == DBNull.Value)
                {
                    throw new InvalidOperationException(
                        "Procedura nije vratila ID novog korisnika.");
                }

                return Convert.ToInt32(rezultat);
            }
        }

        public KorisnikKlasa DajPoKorisnickomImenu(
            string korisnickoIme)
        {
            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand(
                    "dbo.DajKorisnikaPoKorisnickomImenu",
                    konekcija))
            {
                komanda.CommandType = CommandType.StoredProcedure;

                komanda.Parameters.Add(
                    "@KorisnickoIme",
                    SqlDbType.NVarChar,
                    50).Value = korisnickoIme.Trim();

                konekcija.Open();

                using (SqlDataReader reader = komanda.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return KorisnikMapper.Mapiraj(reader,true);
                }
            }
        }

        public List<KorisnikKlasa> DajSveKorisnike()
        {
            List<KorisnikKlasa> korisnici =
                new List<KorisnikKlasa>();

            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand("dbo.DajSveKorisnike", konekcija))
            {
                komanda.CommandType = CommandType.StoredProcedure;

                konekcija.Open();

                using (SqlDataReader reader = komanda.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        korisnici.Add(
                        KorisnikMapper.Mapiraj(reader,false));
                    }
                }
            }

            return korisnici;
        }

        public void AzurirajDatumPoslednjePrijave(int korisnikID)
        {
            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand(
                    "dbo.AzurirajDatumPoslednjePrijave",
                    konekcija))
            {
                komanda.CommandType = CommandType.StoredProcedure;

                komanda.Parameters.Add(
                    "@ID",
                    SqlDbType.Int).Value = korisnikID;

                konekcija.Open();
                komanda.ExecuteNonQuery();
            }
        }

        
        
    }
}