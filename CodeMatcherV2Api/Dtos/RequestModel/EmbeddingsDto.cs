using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMatcherV2Api.Dtos.RequestDtos
{
    public class EmbeddingsDto : AuditEntity
    {
        [Key]
        public int Id { get; set; }

        public int RunTypeId { get; set; }

        [ForeignKey("RunTypeId")]
        public LookupDto RunType { get; set; }

        public int SegmentTypeId { get; set; }

        [ForeignKey("SegmentTypeId")]
        public LookupDto SegmentType { get; set; }

        public int EmbeddingFrequencyId { get; set; }

        [ForeignKey("EmbeddingFrequencyId")]
        public LookupDto EmbeddingFrequency { get; set; }

        public string RunSchedule { get; set; }
    }
}
