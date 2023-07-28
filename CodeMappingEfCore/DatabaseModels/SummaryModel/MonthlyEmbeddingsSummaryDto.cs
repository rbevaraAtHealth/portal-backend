using CodeMappingEfCore.DatabaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables
{
    public class MonthlyEmbeddingsSummaryDto : AuditEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid TaskId { get; set; }
        [Required]
        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public CodeMappingRequestDto CodeMappingRequest { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Segment { get; set; }
        [Required]
        public int NoOfRecordsImportedFromDatabase { get; set; }
        [Required]
        public int NoOfRecordsEmbeddingCreated { get; set; }
    }
}
