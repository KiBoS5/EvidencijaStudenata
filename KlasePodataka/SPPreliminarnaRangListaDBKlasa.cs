using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KlasePodataka.Mapiranja;

namespace KlasePodataka
{
    public class SPPreliminarnaRangListaDBKlasa
    {
        private readonly string _stringKonekcije;

        public SPPreliminarnaRangListaDBKlasa(
            string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public void ObjaviPreliminarnuRangListu(
            List<PreliminarnaRangListaKlasa> stavke,
            int objavioKorisnikID)
        {
            DataTable tabelaStavki =
                NapraviTabeluStavki(stavke);

            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand(
                    "dbo.ObjaviPreliminarnuRangListu",
                    konekcija))
            {
                komanda.CommandType =
                    CommandType.StoredProcedure;

                komanda.Parameters.Add(
                    "@ObjavioKorisnikID",
                    SqlDbType.Int).Value =
                        objavioKorisnikID;

                SqlParameter stavkeParametar =
                    komanda.Parameters.AddWithValue(
                        "@Stavke",
                        tabelaStavki);

                stavkeParametar.SqlDbType =
                    SqlDbType.Structured;

                stavkeParametar.TypeName =
                    "dbo.PreliminarnaRangListaTip";

                konekcija.Open();
                komanda.ExecuteNonQuery();
            }
        }

        public List<PreliminarnaRangListaKlasa>
            DajPreliminarnuRangListu()
        {
            List<PreliminarnaRangListaKlasa> lista =
                new List<PreliminarnaRangListaKlasa>();

            using (SqlConnection konekcija =
                new SqlConnection(_stringKonekcije))
            using (SqlCommand komanda =
                new SqlCommand(
                    "dbo.DajPreliminarnuRangListu",
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
                                PreliminarnaRangListaMapper.Mapiraj(reader));
                    }
                }
            }

            return lista;
        }

        private DataTable NapraviTabeluStavki(
            List<PreliminarnaRangListaKlasa> stavke)
        {
            DataTable tabela = new DataTable();

            tabela.Columns.Add(
                "Pozicija",
                typeof(int));

            tabela.Columns.Add(
                "StudentID",
                typeof(int));

            tabela.Columns.Add(
                "Ime",
                typeof(string));

            tabela.Columns.Add(
                "Prezime",
                typeof(string));

            tabela.Columns.Add(
                "BrojIndeksa",
                typeof(string));

            tabela.Columns.Add(
                "StudijskiProgram",
                typeof(string));

            tabela.Columns.Add(
                "GodinaStudija",
                typeof(string));

            tabela.Columns.Add(
                "Prosek",
                typeof(decimal));

            tabela.Columns.Add(
                "BezRoditelja",
                typeof(bool));

            tabela.Columns.Add(
                "DokumentacijaPrihodaDostavljena",
                typeof(bool));

            tabela.Columns.Add(
                "UkupnaPrimanjaDomacinstva",
                typeof(decimal));

            tabela.Columns.Add(
                "UkupnoBodova",
                typeof(decimal));

            foreach (PreliminarnaRangListaKlasa stavka
                in stavke)
            {
                tabela.Rows.Add(
                    stavka.Pozicija,
                    stavka.StudentID,
                    stavka.Ime,
                    stavka.Prezime,
                    stavka.BrojIndeksa,

                    string.IsNullOrWhiteSpace(
                        stavka.StudijskiProgram)
                            ? (object)DBNull.Value
                            : stavka.StudijskiProgram,

                    string.IsNullOrWhiteSpace(
                        stavka.GodinaStudija)
                            ? (object)DBNull.Value
                            : stavka.GodinaStudija,

                    stavka.Prosek,
                    stavka.BezRoditelja,

                    stavka
                        .DokumentacijaPrihodaDostavljena,

                    stavka.UkupnaPrimanjaDomacinstva,
                    stavka.UkupnoBodova);
            }

            return tabela;
        }

        
        
    }
}