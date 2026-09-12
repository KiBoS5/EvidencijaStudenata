using KlasePodataka;
using System.Collections.Generic;

namespace PrezentacionaLogika
{
    public class FormaStudentKlasa
    {
        private string _konekcija;

        public FormaStudentKlasa(string konekcija)
        {
            _konekcija = konekcija;
        }

        public List<StudentKlasa> DajSveStudente()
        {
            return new SPStudentDBKlasa(_konekcija).DajSveStudente();
        }

        public int DodajStudenta(StudentKlasa student, int maksimalanBrojPrijava)
        {
            return new SPStudentDBKlasa(_konekcija).DodajStudenta(student, maksimalanBrojPrijava);
        }
    }
}
