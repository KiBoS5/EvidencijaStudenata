using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtils
{
    public class KonekcijaKlasa
    {
        private SqlConnection _konekcija;

        private string _putanjaBaze;
        private string _nazivBaze;
        private string _nazivDBMSinstance;
        private string _stringKonekcije;

        // Konstruktor 1
        public KonekcijaKlasa(string nazivDBMSInstance, string putanjaBaze, string nazivBaze)
        {
            _putanjaBaze = putanjaBaze;
            _nazivBaze = nazivBaze;
            _nazivDBMSinstance = nazivDBMSInstance;
            _stringKonekcije = "";
        }

        // Konstruktor 2 (direktan connection string)
        public KonekcijaKlasa(string noviStringKonekcije)
        {
            _putanjaBaze = "";
            _nazivBaze = "";
            _nazivDBMSinstance = "";
            _stringKonekcije = noviStringKonekcije;
        }

        private string DajStringKonekcije()
        {
            string pomStringKonekcije;

            if (string.IsNullOrEmpty(_stringKonekcije))
            {
                if (string.IsNullOrEmpty(_putanjaBaze))
                {
                    pomStringKonekcije = "Data Source=" + _nazivDBMSinstance +
                                         ";Initial Catalog=" + _nazivBaze +
                                         ";Integrated Security=True";
                }
                else
                {
                    pomStringKonekcije = "Data Source=.\\" + _nazivDBMSinstance +
                                         ";AttachDbFilename=" + _putanjaBaze + "\\" + _nazivBaze +
                                         ";Integrated Security=True;Connect Timeout=30;User Instance=True";
                }
            }
            else
            {
                pomStringKonekcije = _stringKonekcije;
            }

            return pomStringKonekcije;
        }

        public bool OtvoriKonekciju()
        {
            bool uspeh;

            _konekcija = new SqlConnection();
            _konekcija.ConnectionString = DajStringKonekcije();

            try
            {
                _konekcija.Open();
                uspeh = true;
            }
            catch
            {
                uspeh = false;
            }

            return uspeh;
        }

        public SqlConnection DajKonekciju()
        {
            return _konekcija;
        }

        public void ZatvoriKonekciju()
        {
            _konekcija.Close();
            _konekcija.Dispose();
        }
    }
}
