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
        public string TaskId { get; set; } = null!;
        [Required]
        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public CodeMappingRequestDto CodeMappingRequest { get; set; } = null!;
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Segment { get; set; } = null!;
        [Required]
        public string Threshold { get; set; } = null!;
        [Required]
        public int NoOfBaseRecords { get; set; }
        [Required]
        public int NoOfInputRecords { get; set; }
        [Required]
        public int NoOfNoiseRecords { get; set; }
        [Required]
        public int NoOfProcessedRecords { get; set; }
        [Required]
        public int NoOfRecordsForWhichCodeGenerated { get; set; }
        [Required]
        public int NoOfRecordsForWhichCodeNotGenerated { get; set; }
        [Required]
        public string StartLink { get; set; } = null!;
        [Required]
        public string LatestLink { get; set; } = null!;

        public string? UploadCsvOutputDirPath { get; set; } 
    }
}
