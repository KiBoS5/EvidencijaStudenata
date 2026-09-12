using KlasePodataka;
using System.Collections.Generic;

namespace Repozitorijumi
{
    public interface IStudentRepository
    {
        List<StudentKlasa> GetAll();
        int DodajStudenta(StudentKlasa student,int maksimalanBrojPrijava);
    }
}
