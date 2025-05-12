using PatientRestApi.DAL.Repository.Interfaces.Patients.Models;
using Microsoft.EntityFrameworkCore;

namespace PatientRestApi.DAL.Repository.Interfaces.Context
{
    public class PatientDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientName> PatientNames { get; set; }
        public DbSet<GivenName> GivenNames { get; set; }
        

        public PatientDbContext(DbContextOptions<PatientDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Name)
                .WithOne(n => n.Patient)
                .HasForeignKey<PatientName>(n => n.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PatientName>()
                .HasMany(n => n.Given)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
