using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables;
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

            modelBuilder.Entity<CodeMappingRequestDto>().HasOne(s=>s.RunType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CodeMappingRequestDto>().HasOne(s=>s.SegmentType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CodeMappingRequestDto>().HasOne(s => s.CodeMappingType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<LookupTypeDto> LookupTypes { get; set; }
        public DbSet<LookupDto> Lookups { get; set; }
        public DbSet<CodeMappingRequestDto> CodeMappingRequests { get; set; }
        public DbSet<CodeMappingResponseDto> CodeMappingResponses { get; set; }
        public DbSet<CodeMappingDto> CodeMappings { get; set; }
        public DbSet<CodeGenerationSummaryDto>CodeGenerationSummary { get; set; }
        public DbSet<MonthlyEmbeddingsSummaryDto>MonthlyEmbeddingsSummary { get; set; }
        public DbSet<WeeklyEmbeddingsSummaryDto>WeeklyEmbeddingsSummary { get; set; }
    }
}
