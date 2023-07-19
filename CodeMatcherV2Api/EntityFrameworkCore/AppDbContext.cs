using CodeMatcherV2Api.Dtos;
using CodeMatcherV2Api.Dtos.RequestDtos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CodeMatcherV2Api.EntityFrameworkCore
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LookupDto>().HasOne(s => s.LookupType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CodeGenerationDto>().HasOne(s=>s.RunType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CodeGenerationDto>().HasOne(s=>s.SegmentType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmbeddingsDto>().HasOne(s => s.RunType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmbeddingsDto>().HasOne(s => s.SegmentType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmbeddingsDto>().HasOne(s => s.EmbeddingFrequency)
                .WithMany().OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<LookupTypeDto> LookupTypes { get; set; }
        public DbSet<LookupDto> Lookups { get; set; }
        public DbSet<CodeGenerationDto> CodeGenerations { get; set; }
        public DbSet<EmbeddingsDto> Embeddings { get; set; }
    }
}
