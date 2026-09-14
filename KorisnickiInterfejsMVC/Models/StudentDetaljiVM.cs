using System;
using System.Collections.Generic;

namespace KorisnickiInterfejsMVC.Models
{
    public class StudentDetaljiVM
    {
        public int StudentID { get; set; }

        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }

        public string Email { get; set; }
        public string Telefon { get; set; }

        public string BrojIndeksa { get; set; }
        public string StudijskiProgram { get; set; }
        public string GodinaStudija { get; set; }

        public decimal Prosek { get; set; }
        public bool BezRoditelja { get; set; }

        public decimal UkupnaPrimanjaDomacinstva { get; set; }
        public int BrojClanovaPorodice { get; set; }

        public List<StudentDokumentVM> Dokumenti { get; set; }

        public StudentDetaljiVM()
        {
            Dokumenti = new List<StudentDokumentVM>();
        }
    }
}