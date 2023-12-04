using Microsoft.EntityFrameworkCore;
using PublicUtilities.Data.Entities;

#pragma warning disable CS8618
namespace PublicUtilities.Data
{
    public class PublicUtilitiesDbContext : DbContext
    {
        public PublicUtilitiesDbContext() { }

        public PublicUtilitiesDbContext(DbContextOptions<PublicUtilitiesDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=PublicUtilities;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationWorker>().HasKey(applicationWorker => new { applicationWorker.WorkerId, applicationWorker.ApplicationId });
        }

        public DbSet<Worker> Workers { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationWorker> ApplicationWorkers { get; set; }
    }
}
