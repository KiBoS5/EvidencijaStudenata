using System;

namespace KlasePodataka
{
    /// <summary>
    /// Configuration class for scholarship application scoring thresholds
    /// Allows easy modification of scoring rules without code changes
    /// </summary>
    public class SkoringKonfiguraciaKlasa
    {
        public int ID { get; set; }
        
        // GPA Scoring Thresholds
        public decimal MinimalanProsek { get; set; } = 8.50m;
        public decimal ProsekZa10Bodova { get; set; } = 8.50m;
        public decimal ProsekZa15Bodova { get; set; } = 9.00m;
        public decimal ProsekZa20Bodova { get; set; } = 9.50m;
        
        // Financial Status Scoring - income ranges (in currency units)
        public decimal PrimanjaZa10Bodova { get; set; } = 1000m;      // <= this = 10 points
        public decimal PrimanjaZa1Bod { get; set; } = 11000m;         // >= this = 1 point
        
        // Year of Study Points
        public int BodoviGodinaStudije1 { get; set; } = 2;
        public int BodoviGodinaStudije2 { get; set; } = 4;
        public int BodoviGodinaStudije3 { get; set; } = 6;
        public int BodoviGodinaStudije4 { get; set; } = 8;
        public int BodoviMaster { get; set; } = 10;
        
        // Orphan Status Bonus (multiplier for financial score)
        public decimal BezRoditeljaMultiplier { get; set; } = 0.8m;
        
        // Audit fields
        public DateTime DatumKreiranja { get; set; } = DateTime.Now;
        public DateTime DatumAžuriranja { get; set; } = DateTime.Now;
    }
}
