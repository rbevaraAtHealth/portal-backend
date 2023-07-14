using CodeMatcherV2Api.BusinessLayer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMatcherV2Api.Dtos
{
    public class CodeGenerationDto : AuditEntity
    {
        [Key]
        public int Id { get; set; }

        public int RunTypeId { get; set; }

        [ForeignKey("RunTypeId")]
        public LookupDto RunType { get; set; }

        public int SegmentTypeId { get; set; }

        [ForeignKey("SegmentTypeId")]
        public LookupDto SegmentType { get; set; }

        public string RunSchedule { get; set; }

        public string Threshold { get; set; }

        public string LatestLink { get; set; }

        public string ClientId { get; set; }
    }
}
