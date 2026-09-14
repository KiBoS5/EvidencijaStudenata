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

        public StudentKlasa DajStudentaPoID(int studentID)
        {
            var db = new SPStudentDBKlasa(_konekcija);

            return db.DajStudentaPoID(studentID);
        }

        public int DajBrojPrijava()
        {
            var tabela = new StudentTabelaKlasa(_konekcija);

            return tabela.DajBrojPrijava();
        }

        public void IzmeniStudenta(StudentKlasa student)
        {
            var db = new SPStudentDBKlasa(_konekcija);

            db.IzmeniStudenta(student);
        }

        public void ObrisiStudenta(int studentID)
        {
            var db = new SPStudentDBKlasa(_konekcija);

            db.ObrisiStudenta(studentID);
        }

    }

}
