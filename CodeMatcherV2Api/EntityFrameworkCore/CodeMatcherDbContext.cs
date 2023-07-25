using CodeMatcherV2Api.Dtos;
using CodeMatcherV2Api.Dtos.RequestDtos;
using CodeMatcherV2Api.Dtos.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CodeMatcherV2Api.EntityFrameworkCore
{
    public class CodeMatcherDbContext : DbContext
    {
        public CodeMatcherDbContext(DbContextOptions<CodeMatcherDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LookupDto>().HasOne(s => s.LookupType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CodeGenerationRequestDto>().HasOne(s=>s.RunType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CodeGenerationRequestDto>().HasOne(s=>s.SegmentType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmbeddingsRequestDto>().HasOne(s => s.RunType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmbeddingsRequestDto>().HasOne(s => s.SegmentType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmbeddingsRequestDto>().HasOne(s => s.EmbeddingFrequency)
                .WithMany().OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<LookupTypeDto> LookupTypes { get; set; }
        public DbSet<LookupDto> Lookups { get; set; }
        public DbSet<CodeGenerationRequestDto> CodeGenerationRequests { get; set; }
        public DbSet<EmbeddingsRequestDto> EmbeddingRequests { get; set; }
        public DbSet<CodeGenerationResponseDto> CodeGenerationResponses { get; set; }
        public DbSet<EmbeddingsResponseDto> EmbeddingsResponses { get; set; }
    }
}
