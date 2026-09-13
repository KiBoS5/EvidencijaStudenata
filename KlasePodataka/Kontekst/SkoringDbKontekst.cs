using System.Data.Entity;

namespace KlasePodataka.Kontekst
{
    public class SkoringDbKontekst : DbContext
    {
        static SkoringDbKontekst()
        {
            // Koristimo postojeću bazu.
            // EF ne kreira, ne briše i ne menja njenu strukturu.
            Database.SetInitializer<SkoringDbKontekst>(null);
        }

        public SkoringDbKontekst(string stringKonekcije)
            : base(stringKonekcije)
        {
        }

        public DbSet<SkoringKonfiguracijaKlasa>
            SkoringKonfiguracije
        { get; set; }

        protected override void OnModelCreating(
            DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var konfiguracija =
                modelBuilder.Entity<SkoringKonfiguracijaKlasa>();

            konfiguracija.Property(x => x.MinimalanProsek)
                .HasPrecision(5, 2);

            konfiguracija.Property(x => x.ProsekZa10Bodova)
                .HasPrecision(5, 2);

            konfiguracija.Property(x => x.ProsekZa15Bodova)
                .HasPrecision(5, 2);

            konfiguracija.Property(x => x.ProsekZa20Bodova)
                .HasPrecision(5, 2);

            konfiguracija.Property(x => x.PrimanjaZa10Bodova)
                .HasPrecision(15, 2);

            konfiguracija.Property(x => x.PrimanjaZa1Bod)
                .HasPrecision(15, 2);

            konfiguracija.Property(x => x.BezRoditeljaMultiplier)
                .HasPrecision(3, 2);

            konfiguracija.Property(x => x.DatumKreiranja)
                .HasColumnType("datetime2");

            konfiguracija.Property(x => x.DatumAzuriranja)
                .HasColumnType("datetime2");
        }
    }
}