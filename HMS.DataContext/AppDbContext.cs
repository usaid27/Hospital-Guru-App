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

        public DbSet<Details> Details { get; set; }
        public DbSet<HospitalDto> HospitalDto { get; set; }
        public DbSet<ProcedureDto> ProcedureDto { get; set; }
        public DbSet<DoctorsDto> DoctorsDto { get; set; }
    }
}
