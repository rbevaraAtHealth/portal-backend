using CodeMappingEfCore.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables
{
    public class CodeGenerationSummaryDto:ClientAuditEntity
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
        public float Threshold { get; set; }
        [Required]
        public int NoOfBaseRecords { get; set; }
        [Required]
        public int NoOfInputRecords { get; set; }
        [Required]
        public int NoOfNoiseRecords { get;}
        [Required]
        public int NoOfProcessedRecords { get; set; }
        [Required]
        public int NoOfRecordsForWhichCodeGenerated { get; set; }
        [Required]
        public int NoOfRecordsForWhichCodeNotGenerated { get; set; }
        [Required]
        public string StartLink { get; set; }
        [Required]
        public string LatestLink { get; set; }

    }
}
