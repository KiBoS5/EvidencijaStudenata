using System.Collections.Generic;
using KlasePodataka;

namespace Repozitorijumi
{
    public interface IStudentDokumentRepository
    {
        List<StudentDokumentKlasa> DajDokumenteStudenta(
            int studentID);

        void OznaciPodnetim(
            int studentID,
            int dokumentID);

        void ProveriDokument(
            int studentID,
            int dokumentID,
            bool ispravan,
            int proverioKorisnikID,
            string napomena);
    }
}