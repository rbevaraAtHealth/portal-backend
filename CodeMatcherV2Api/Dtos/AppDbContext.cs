using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace CodeMatcherV2Api.Dtos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<LookupTypeDto> LookupTypes { get; set; }
        public DbSet<LookupDto> Lookups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<LookupTypeDto>().HasData(
                new LookupTypeDto()
                {
                    LookupTypeId = 1,
                    LookupTypeKey = "Segment",
                    LookupTypeDescription = "This lookup defines the segments while creating the jobs.",
                });

            modelBuilder.Entity<LookupDto>().HasData(
                new LookupDto()
                {
                    Id = 1,
                    LookupTypeId = 1,
                    Name = "School",
                });

            modelBuilder.Entity<LookupDto>().HasData(
                new LookupDto()
                {
                    Id = 2,
                    LookupTypeId = 1,
                    Name = "Insurance",
                });

            modelBuilder.Entity<LookupDto>().HasData(
                new LookupDto()
                {
                    Id = 3,
                    LookupTypeId = 1,
                    Name = "State License",
                });

            modelBuilder.Entity<LookupDto>().HasData(
                new LookupDto()
                {
                    Id = 4,
                    LookupTypeId = 1,
                    Name = "Hospital",
                });
        }
    }
}
