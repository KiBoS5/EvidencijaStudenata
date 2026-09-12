using System;
using System.Data;
using System.Data.SqlClient;

namespace KlasePodataka
{
    public class SPSkoringKonfiguraciaDBKlasa
    {
        private string _stringKonekcije;

        public SPSkoringKonfiguraciaDBKlasa(string noviStringKonekcije)
        {
            _stringKonekcije = noviStringKonekcije;
        }

        // =========================
        // GET CURRENT CONFIGURATION
        // =========================
        public SkoringKonfiguraciaKlasa DajKonfiguraciju()
        {
            SkoringKonfiguraciaKlasa config = null;

            using (SqlConnection konekcija = new SqlConnection(_stringKonekcije))
            {
                konekcija.Open();

                using (SqlCommand komanda = new SqlCommand("DajSkoringKonfiguraciju", konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = komanda.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            config = MapirajKonfiguraciju(reader);
                        }
                    }
                }
            }

            // Return default if none exists
            return config ?? new SkoringKonfiguraciaKlasa();
        }

        // =========================
        // UPDATE CONFIGURATION
        // =========================
        public bool AžurirajKonfiguraciju(SkoringKonfiguraciaKlasa config)
        {
            int rezultat = 0;

            using (SqlConnection konekcija = new SqlConnection(_stringKonekcije))
            {
                konekcija.Open();

                using (SqlCommand komanda = new SqlCommand("AžurirajSkoringKonfiguraciju", konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;

                    komanda.Parameters.Add("@MinimalanProsek", SqlDbType.Decimal).Value = config.MinimalanProsek;
                    komanda.Parameters.Add("@ProsekZa10Bodova", SqlDbType.Decimal).Value = config.ProsekZa10Bodova;
                    komanda.Parameters.Add("@ProsekZa15Bodova", SqlDbType.Decimal).Value = config.ProsekZa15Bodova;
                    komanda.Parameters.Add("@ProsekZa20Bodova", SqlDbType.Decimal).Value = config.ProsekZa20Bodova;
                    komanda.Parameters.Add("@PrimanjaZa10Bodova", SqlDbType.Decimal).Value = config.PrimanjaZa10Bodova;
                    komanda.Parameters.Add("@PrimanjaZa1Bod", SqlDbType.Decimal).Value = config.PrimanjaZa1Bod;
                    komanda.Parameters.Add("@BodoviGodinaStudije1", SqlDbType.Int).Value = config.BodoviGodinaStudije1;
                    komanda.Parameters.Add("@BodoviGodinaStudije2", SqlDbType.Int).Value = config.BodoviGodinaStudije2;
                    komanda.Parameters.Add("@BodoviGodinaStudije3", SqlDbType.Int).Value = config.BodoviGodinaStudije3;
                    komanda.Parameters.Add("@BodoviGodinaStudije4", SqlDbType.Int).Value = config.BodoviGodinaStudije4;
                    komanda.Parameters.Add("@BodoviMaster", SqlDbType.Int).Value = config.BodoviMaster;
                    komanda.Parameters.Add("@BezRoditeljaMultiplier", SqlDbType.Decimal).Value = config.BezRoditeljaMultiplier;

                    rezultat = komanda.ExecuteNonQuery();
                }
            }

            return (rezultat > 0);
        }

        private SkoringKonfiguraciaKlasa MapirajKonfiguraciju(SqlDataReader reader)
        {
            return new SkoringKonfiguraciaKlasa
            {
                ID = Convert.ToInt32(reader["ID"]),
                MinimalanProsek = Convert.ToDecimal(reader["MinimalanProsek"]),
                ProsekZa10Bodova = Convert.ToDecimal(reader["ProsekZa10Bodova"]),
                ProsekZa15Bodova = Convert.ToDecimal(reader["ProsekZa15Bodova"]),
                ProsekZa20Bodova = Convert.ToDecimal(reader["ProsekZa20Bodova"]),
                PrimanjaZa10Bodova = Convert.ToDecimal(reader["PrimanjaZa10Bodova"]),
                PrimanjaZa1Bod = Convert.ToDecimal(reader["PrimanjaZa1Bod"]),
                BodoviGodinaStudije1 = Convert.ToInt32(reader["BodoviGodinaStudije1"]),
                BodoviGodinaStudije2 = Convert.ToInt32(reader["BodoviGodinaStudije2"]),
                BodoviGodinaStudije3 = Convert.ToInt32(reader["BodoviGodinaStudije3"]),
                BodoviGodinaStudije4 = Convert.ToInt32(reader["BodoviGodinaStudije4"]),
                BodoviMaster = Convert.ToInt32(reader["BodoviMaster"]),
                BezRoditeljaMultiplier = Convert.ToDecimal(reader["BezRoditeljaMultiplier"]),
                DatumKreiranja = Convert.ToDateTime(reader["DatumKreiranja"]),
                DatumAžuriranja = Convert.ToDateTime(reader["DatumAžuriranja"])
            };
        }
    }
}
