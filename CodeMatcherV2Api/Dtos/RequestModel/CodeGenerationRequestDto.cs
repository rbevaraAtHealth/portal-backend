using CodeMatcherV2Api.BusinessLayer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMatcherV2Api.Dtos.RequestDtos
{
    public class CodeGenerationRequestDto : ClientAuditEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RunTypeId { get; set; }

        [ForeignKey("RunTypeId")]
        public LookupDto RunType { get; set; }

        [Required]
        public int SegmentTypeId { get; set; }

        [ForeignKey("SegmentTypeId")]
        public LookupDto SegmentType { get; set; }

        public string RunSchedule { get; set; }

        public string Threshold { get; set; }

        public string LatestLink { get; set; }
    }
}
