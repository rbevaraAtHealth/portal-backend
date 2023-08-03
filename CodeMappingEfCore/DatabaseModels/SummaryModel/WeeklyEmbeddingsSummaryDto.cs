using CodeMappingEfCore.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables
{
    public class WeeklyEmbeddingsSummaryDto:AuditEntity
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
        public int NoOfBaseRecordsImportedFromDatabase { get; set; }
        [Required]
        public int NoOfRecordsEmbeddingsCreated { get; set; }
        [Required]
        public int NoOfBaseRecordsBeforeRun { get; set; }
        [Required]
        public int NoOfRecordsAfterRun { get; set; }
        [Required]
        public int StartLink { get; set; }
        [Required]
        public int LatestLink { get; set; }
    }
}
