using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlasePodataka
{
    [Table("SkoringKonfiguracija", Schema = "dbo")]
    public class SkoringKonfiguracijaKlasa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public decimal MinimalanProsek { get; set; }

        [Required]
        public decimal ProsekZa10Bodova { get; set; }

        [Required]
        public decimal ProsekZa15Bodova { get; set; }

        [Required]
        public decimal ProsekZa20Bodova { get; set; }

        [Required]
        public decimal PrimanjaZa10Bodova { get; set; }

        [Required]
        public decimal PrimanjaZa1Bod { get; set; }

        [Required]
        public int BodoviGodinaStudije1 { get; set; }

        [Required]
        public int BodoviGodinaStudije2 { get; set; }

        [Required]
        public int BodoviGodinaStudije3 { get; set; }

        [Required]
        public int BodoviGodinaStudije4 { get; set; }

        [Required]
        public int BodoviMaster { get; set; }

        [Required]
        public decimal BezRoditeljaMultiplier { get; set; }

        public System.DateTime? DatumKreiranja { get; set; }

        public System.DateTime? DatumAzuriranja { get; set; }
    }
}