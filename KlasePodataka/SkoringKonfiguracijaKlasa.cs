namespace KlasePodataka
{
    public class SkoringKonfiguracijaKlasa
    {
        public int ID { get; set; }

        public decimal MinimalanProsek { get; set; }
        public decimal ProsekZa10Bodova { get; set; }
        public decimal ProsekZa15Bodova { get; set; }
        public decimal ProsekZa20Bodova { get; set; }

        public decimal PrimanjaZa10Bodova { get; set; }
        public decimal PrimanjaZa1Bod { get; set; }

        public int BodoviGodinaStudije1 { get; set; }
        public int BodoviGodinaStudije2 { get; set; }
        public int BodoviGodinaStudije3 { get; set; }
        public int BodoviGodinaStudije4 { get; set; }
        public int BodoviMaster { get; set; }

        public decimal BezRoditeljaMultiplier { get; set; }
    }
}