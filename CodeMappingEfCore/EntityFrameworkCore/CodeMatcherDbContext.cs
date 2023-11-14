using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.EntityFrameworkCore.DatabaseModels;
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

            modelBuilder.Entity<CodeMappingRequestDto>().HasOne(s => s.RunType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CodeMappingRequestDto>().HasOne(s => s.SegmentType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CodeMappingRequestDto>().HasOne(s => s.CodeMappingType)
                .WithMany().OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<CodeGenerationSummaryDto>().HasOne(s => s.CodeMappingRequest)
            //    .WithMany().OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<MonthlyEmbeddingsSummaryDto>().HasOne(s => s.CodeMappingRequest)
            //    .WithOne().OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<WeeklyEmbeddingsSummaryDto>().HasOne(s => s.CodeMappingRequest)
            //    .WithOne().OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<LookupTypeDto> LookupTypes { get; set; } = null!;
        public DbSet<LookupDto> Lookups { get; set; } = null!;
        public DbSet<CodeMappingRequestDto> CodeMappingRequests { get; set; } = null!;
        public DbSet<CodeMappingResponseDto> CodeMappingResponses { get; set; } = null!;
        public DbSet<CodeMappingDto> CodeMappings { get; set; } = null!;
        public DbSet<CodeGenerationSummaryDto>CodeGenerationSummary { get; set; } = null!;
        public DbSet<MonthlyEmbeddingsSummaryDto>MonthlyEmbeddingsSummary { get; set; } = null!;
        public DbSet<WeeklyEmbeddingsSummaryDto>WeeklyEmbeddingsSummary { get; set; } = null!;
        public DbSet<CodeGenerationOverwriteDto> CodeGenerationOverwrites { get; set; } = null!;
        public DbSet<CodeGenerationOverwriteHistoryDto> CodeGenerationOverwriteHistory { get; set; } = null!;
        public DbSet<UserDto> UserDetail { get; set; } = null!;
        public DbSet<LogTableDto> LogTable { get; set; } = null!;
        public DbSet<APIKeyDto> ApiKeys { get; set; } = null!;
    }
}
