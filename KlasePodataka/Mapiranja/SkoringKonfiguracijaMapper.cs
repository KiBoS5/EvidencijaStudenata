using System;
using System.Data.SqlClient;

namespace KlasePodataka.Mapiranja
{
    public static class SkoringKonfiguracijaMapper
    {
        public static SkoringKonfiguracijaKlasa Mapiraj(
            SqlDataReader reader)
        {
            return new SkoringKonfiguracijaKlasa
            {
                ID = Convert.ToInt32(reader["ID"]),

                MinimalanProsek =
                    Convert.ToDecimal(reader["MinimalanProsek"]),

                ProsekZa10Bodova =
                    Convert.ToDecimal(reader["ProsekZa10Bodova"]),

                ProsekZa15Bodova =
                    Convert.ToDecimal(reader["ProsekZa15Bodova"]),

                ProsekZa20Bodova =
                    Convert.ToDecimal(reader["ProsekZa20Bodova"]),

                PrimanjaZa10Bodova =
                    Convert.ToDecimal(reader["PrimanjaZa10Bodova"]),

                PrimanjaZa1Bod =
                    Convert.ToDecimal(reader["PrimanjaZa1Bod"]),

                BodoviGodinaStudije1 =
                    Convert.ToInt32(reader["BodoviGodinaStudije1"]),

                BodoviGodinaStudije2 =
                    Convert.ToInt32(reader["BodoviGodinaStudije2"]),

                BodoviGodinaStudije3 =
                    Convert.ToInt32(reader["BodoviGodinaStudije3"]),

                BodoviGodinaStudije4 =
                    Convert.ToInt32(reader["BodoviGodinaStudije4"]),

                BodoviMaster =
                    Convert.ToInt32(reader["BodoviMaster"]),

                BezRoditeljaMultiplier =
                    Convert.ToDecimal(reader["BezRoditeljaMultiplier"]),
            };
        }
    }
}