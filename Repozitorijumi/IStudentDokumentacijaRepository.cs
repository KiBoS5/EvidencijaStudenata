using System.Collections.Generic;
using KlasePodataka;

namespace Repozitorijumi
{
    public interface IStudentDokumentacijaRepository
    {
        List<StudentDokumentacijaKlasa>
            DajDokumentacijuStudenata();

        void OznaciDokumentacijuPodnetom(
            int studentID);

        void ProveriDokumentacijuStudenta(
            int studentID,
            bool dokumentacijaIspravna,
            string napomena,
            int proverioKorisnikID);
    }
}