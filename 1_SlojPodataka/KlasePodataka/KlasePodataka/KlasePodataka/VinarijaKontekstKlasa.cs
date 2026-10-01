using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace KlasePodataka
{
    public class VinarijaKontekstKlasa : DbContext
    {
        public VinarijaKontekstKlasa(string stringKonekcije)
            : base(stringKonekcije)
        {
            var _provajder = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            Database.SetInitializer<VinarijaKontekstKlasa>(null);
        }

        public DbSet<SortaEfKlasa> Sorte { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<SortaEfKlasa> sorta = modelBuilder.Entity<SortaEfKlasa>();
            sorta.ToTable("SORTA");
            sorta.HasKey(s => s.Sifra);
            sorta.Property(s => s.Sifra).HasColumnName("Sifra").HasMaxLength(10);
            sorta.Property(s => s.Naziv).HasColumnName("Naziv").HasMaxLength(50);
            base.OnModelCreating(modelBuilder);
        }
    }
}
