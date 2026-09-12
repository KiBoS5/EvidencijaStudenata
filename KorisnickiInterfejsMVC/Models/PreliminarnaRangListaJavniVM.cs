using System.Collections.Generic;

namespace KorisnickiInterfejsMVC.Models
{
    public class PreliminarnaRangListaJavniVM
    {
        public PreliminarnaRangListaJavniVM()
        {
            Stavke =
                new List<PreliminarnaRangListaStavkaVM>();

            Primedba =
                new PrimedbaNaRangListuVM();
        }

        public List<PreliminarnaRangListaStavkaVM>
            Stavke
        { get; set; }

        public PrimedbaNaRangListuVM
            Primedba
        { get; set; }

        public bool ListaJeObjavljena
        {
            get
            {
                return Stavke != null &&
                       Stavke.Count > 0;
            }
        }
    }
}