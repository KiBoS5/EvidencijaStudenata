using System;
using System.Collections.Generic;
using System.Data;
using KlasePodataka;

namespace Repozitorijumi
{
    public class StudentRepositorySP : IStudentRepository
    {
        private string _konekcija;

        public StudentRepositorySP(string konekcija)
        {
            _konekcija = konekcija;
        }

        public List<StudentKlasa> DajSve()
        {
            var db = new SPStudentDBKlasa(_konekcija);
            return db.DajSveStudente();
        }

        public int DodajStudenta(
    StudentKlasa student,
    int maksimalanBrojPrijava)
        {
            SPStudentDBKlasa db =
                new SPStudentDBKlasa(
                    _konekcija);

            return db.DodajStudenta(
                student,
                maksimalanBrojPrijava);
        }
    }
}
