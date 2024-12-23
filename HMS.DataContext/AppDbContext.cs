﻿using HMS.Dto;
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
        public DbSet<MedicalcoreAndSpecialities> MedicalCoreAndSpecialities { get; set; } 
        public DbSet<OtherSpecialities> OtherSpecialities { get; set; }
        public DbSet<SpecialitiesName> SpecialitiesNames { get; set; } 
        public DbSet<ProcedureDto> ProcedureDto { get; set; }
        public DbSet<ProcedureCatagory> ProcedureCatagory { get; set; }
        public DbSet<DoctorEducation> DoctorEducation { get; set; }
        public DbSet<DoctorsDto> DoctorsDto { get; set; }
        public DbSet<ContactDoctor> ContactDoctors {  get; set; }
        public DbSet<ContactHospital> ContactHospital {  get; set; }
        public DbSet<ProcedureTypes> ProcedureType {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
        }
    }
}
