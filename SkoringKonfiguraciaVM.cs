namespace KorisnickiInterfejsMVC.Models
{
    public class SkoringKonfiguraciaVM
    {
        public int ID { get; set; }
        
        // GPA Scoring
        public decimal MinimalanProsek { get; set; }
        public decimal ProsekZa10Bodova { get; set; }
        public decimal ProsekZa15Bodova { get; set; }
        public decimal ProsekZa20Bodova { get; set; }
        
        // Financial Status Scoring
        public decimal PrimanjaZa10Bodova { get; set; }
        public decimal PrimanjaZa1Bod { get; set; }
        
        // Year of Study Points
        public int BodoviGodinaStudije1 { get; set; }
        public int BodoviGodinaStudije2 { get; set; }
        public int BodoviGodinaStudije3 { get; set; }
        public int BodoviGodinaStudije4 { get; set; }
        public int BodoviMaster { get; set; }
        
        // Orphan Status Bonus
        public decimal BezRoditeljaMultiplier { get; set; }
    }
}
