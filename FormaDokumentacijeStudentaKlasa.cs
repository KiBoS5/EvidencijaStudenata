using KlasePodataka;
using System;

namespace PrezentacionaLogika
{
    /// <summary>
    /// Presentation layer for document verification workflow
    /// </summary>
    public class FormaDokumentacijeStudentaKlasa
    {
        private string _konekcija;

        public FormaDokumentacijeStudentaKlasa(string konekcija)
        {
            _konekcija = konekcija;
        }

        /// <summary>
        /// Get document status for a student
        /// </summary>
        public StudentDokumentaciaKlasa DajDokumentacijuStudenta(int studentID)
        {
            var db = new SPStudentDokumentaciaDBKlasa(_konekcija);
            return db.DajDokumentacijuStudenta(studentID);
        }

        /// <summary>
        /// Mark all documents as submitted for a student
        /// </summary>
        public bool Ozna?iDokumentiPodneseni(int studentID)
        {
            var db = new SPStudentDokumentaciaDBKlasa(_konekcija);
            var dokumentacija = db.DajDokumentacijuStudenta(studentID) ?? new StudentDokumentaciaKlasa 
            { 
                StudentID = studentID 
            };

            dokumentacija.SviDokumentiPodneseni = true;
            dokumentacija.DatumPodnosenja = DateTime.Now;

            if (dokumentacija.ID == 0)
                return db.KreirajDokumentaciju(dokumentacija) > 0;
            else
                return db.AžurirajDokumentaciju(dokumentacija);
        }

        /// <summary>
        /// Verify all documents for a student (admin action)
        /// </summary>
        public bool ProveridokumentacijuStudenta(int studentID, string napomena)
        {
            var db = new SPStudentDokumentaciaDBKlasa(_konekcija);
            return db.ProveridokumentacijuStudenta(studentID, napomena);
        }

        /// <summary>
        /// Mark student as ready for data entry (after document verification)
        /// </summary>
        public bool Ozna?iSpremnaZaUnos(int studentID)
        {
            var db = new SPStudentDokumentaciaDBKlasa(_konekcija);
            var dokumentacija = db.DajDokumentacijuStudenta(studentID);

            if (dokumentacija != null)
            {
                dokumentacija.SpremnaZaUnos = true;
                return db.AžurirajDokumentaciju(dokumentacija);
            }

            return false;
        }
    }
}
