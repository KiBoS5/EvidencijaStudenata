using KlasePodataka;
using System.Collections.Generic;

namespace Repozitorijumi
{
    public interface IStudentRepository
    {
        List<StudentKlasa> DajSve();
        int DodajStudenta(StudentKlasa student,int maksimalanBrojPrijava);

        StudentKlasa DajStudentaPoID(int studentID);

        int DajBrojPrijava();

        void IzmeniStudenta(StudentKlasa student);

        void ObrisiStudenta(int studentID);
    }


}
