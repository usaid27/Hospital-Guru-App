using HMS.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace HMS.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options, bool ensureCreated = true)
            : base(options)
        {
            if (ensureCreated)
                Database.EnsureCreated();
        }

        // Define DbSets for each entity
        public DbSet<Details> Details { get; set; }
        public DbSet<HospitalDto> HospitalDto { get; set; }
        public DbSet<MedicalcoreAndSpecialities> MedicalCoreAndSpecialities { get; set; } // Corrected naming to match convention
        public DbSet<OtherSpecialities> OtherSpecialities { get; set; }
        public DbSet<SpecialitiesName> SpecialitiesNames { get; set; } // Corrected naming to match convention
        public DbSet<ProcedureDto> ProcedureDto { get; set; }
        public DbSet<DoctorsDto> DoctorsDto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships using Fluent API

            modelBuilder.Entity<MedicalcoreAndSpecialities>()
                .HasOne(m => m.Hospital) // Navigation property in MedicalcoreAndSpecialities
                .WithMany(h => h.MedicalCoreAndSpecialities) // Navigation property in HospitalDto
                .HasForeignKey(m => m.HospitalDtoId); // Foreign key

            modelBuilder.Entity<OtherSpecialities>()
                .HasOne(o => o.Hospital) // Navigation property in OtherSpecialities
                .WithMany(h => h.OtherSpecialities) // Navigation property in HospitalDto
                .HasForeignKey(o => o.HospitalDtoId); // Foreign key

            modelBuilder.Entity<SpecialitiesName>()
                .HasOne(s => s.OtherSpeciality) // Navigation property in SpecialitiesName
                .WithMany(o => o.SpecialitiesNames) // Navigation property in OtherSpecialities
                .HasForeignKey(s => s.OtherSpecialityId); // Foreign key
        }
    }
}
