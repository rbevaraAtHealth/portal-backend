using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CodeMappingEfCore.DatabaseModels
{
    public class CodeMappingRequestDto : ClientAuditEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RunTypeId { get; set; }

        [ForeignKey("RunTypeId")]
        public LookupDto? RunType { get; set; }

        [Required]
        public int SegmentTypeId { get; set; }

        [ForeignKey("SegmentTypeId")]
        public LookupDto? SegmentType { get; set; }

        public int CodeMappingId { get; set; }

        [ForeignKey("CodeMappingId")]
        public LookupDto? CodeMappingType { get; set; }

        public string? RunSchedule { get; set; }

        public string? Threshold { get; set; }

        public string? LatestLink { get; set; }
        public string? CsvFilePath { get; set; }

    }
}
