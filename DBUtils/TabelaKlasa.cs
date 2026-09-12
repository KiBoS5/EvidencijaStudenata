using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtils
{
    public class TabelaKlasa
    {
        private string _nazivTabele;
        private KonekcijaKlasa _konekcijaObjekat;
        private SqlDataAdapter _adapterObjekat;
        private DataSet _dataSetObjekat;

        public TabelaKlasa(KonekcijaKlasa novaKonekcija, string noviNazivTabele)
        {
            _konekcijaObjekat = novaKonekcija;
            _nazivTabele = noviNazivTabele;
        }

        private void KreirajAdapter(string selectUpit, string insertUpit, string deleteUpit, string updateUpit)
        {
            SqlCommand selectKomanda = new SqlCommand(selectUpit, _konekcijaObjekat.DajKonekciju());
            SqlCommand insertKomanda = new SqlCommand(insertUpit, _konekcijaObjekat.DajKonekciju());
            SqlCommand deleteKomanda = new SqlCommand(deleteUpit, _konekcijaObjekat.DajKonekciju());
            SqlCommand updateKomanda = new SqlCommand(updateUpit, _konekcijaObjekat.DajKonekciju());

            _adapterObjekat = new SqlDataAdapter
            {
                SelectCommand = selectKomanda,
                InsertCommand = insertKomanda,
                DeleteCommand = deleteKomanda,
                UpdateCommand = updateKomanda
            };
        }

        private void KreirajDataset()
        {
            _dataSetObjekat = new DataSet();
            _adapterObjekat.Fill(_dataSetObjekat, _nazivTabele);
        }

        public DataSet DajPodatke(string selectUpit)
        {
            KreirajAdapter(selectUpit, "", "", "");
            KreirajDataset();
            return _dataSetObjekat;
        }

        public int DajBrojSlogova()
        {
            return _dataSetObjekat.Tables[0].Rows.Count;
        }

        public bool IzvrsiAzuriranje(string upit)
        {
            bool uspeh = false;
            SqlTransaction transakcija = null;

            try
            {
                SqlConnection konekcija = _konekcijaObjekat.DajKonekciju();
                SqlCommand komanda = konekcija.CreateCommand();

                transakcija = konekcija.BeginTransaction();
                komanda.Transaction = transakcija;

                komanda.CommandText = upit;
                komanda.ExecuteNonQuery();

                transakcija.Commit();
                uspeh = true;
            }
            catch
            {
                transakcija?.Rollback();
                uspeh = false;
            }

            return uspeh;
        }

        public bool IzvrsiAzuriranje(List<string> listaUpita)
        {
            bool uspeh = false;
            SqlTransaction transakcija = null;

            try
            {
                SqlConnection konekcija = _konekcijaObjekat.DajKonekciju();
                SqlCommand komanda = konekcija.CreateCommand();

                transakcija = konekcija.BeginTransaction();
                komanda.Transaction = transakcija;

                foreach (var upit in listaUpita)
                {
                    komanda.CommandText = upit;
                    komanda.ExecuteNonQuery();
                }

                transakcija.Commit();
                uspeh = true;
            }
            catch
            {
                transakcija?.Rollback();
                uspeh = false;
            }

            return uspeh;
        }
    }
}
