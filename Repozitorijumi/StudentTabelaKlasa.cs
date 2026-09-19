using System;
using System.Data;
using DBUtils;

namespace Repozitorijumi
{
    public class StudentTabelaKlasa : TabelaKlasa
    {
        private readonly KonekcijaKlasa _konekcija;

        public StudentTabelaKlasa(string stringKonekcije)
            : this(new KonekcijaKlasa(stringKonekcije))
        {
        }

        private StudentTabelaKlasa(KonekcijaKlasa konekcija)
            : base(konekcija, "Studenti")
        {
            _konekcija = konekcija;
        }

        public int DajBrojPrijava()
        {
            try
            {
                if (!_konekcija.OtvoriKonekciju())
                {
                    throw new InvalidOperationException(
                        "Nije moguće otvoriti konekciju za brojanje prijava.");
                }

                using (DataSet podaci = DajPodatke(
                    "SELECT COUNT(*) AS BrojPrijava FROM dbo.Studenti;"))
                {
                    return Convert.ToInt32(
                        podaci.Tables["Studenti"]
                            .Rows[0]["BrojPrijava"]);
                }
            }
            finally
            {
                if (_konekcija.DajKonekciju() != null)
                {
                    _konekcija.ZatvoriKonekciju();
                }
            }
        }
    }
}