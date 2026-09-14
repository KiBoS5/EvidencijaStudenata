using System;
using System.Collections.Generic;
using KlasePodataka;
using Repozitorijumi;

namespace PrezentacionaLogika
{
    public class FormaPreliminarnaRangListaKlasa
    {
        private readonly IPreliminarnaRangListaRepository _repo;

        public FormaPreliminarnaRangListaKlasa(
            IPreliminarnaRangListaRepository repo)
        {
            if (repo == null)
            {
                throw new ArgumentNullException(nameof(repo));
            }

            _repo = repo;
        }

        public List<PreliminarnaRangListaKlasa>
            DajObjavljenuListu()
        {
            return _repo.DajObjavljenuListu();
        }
    }
}