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
        public DbSet<CodeGenerationDto> CodeGenerations{ get; set; }
        public DbSet<EmbeddingsDto> Embeddings { get; set; }
    }
}
