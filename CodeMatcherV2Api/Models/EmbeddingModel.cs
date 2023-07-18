using System.ComponentModel.DataAnnotations;

namespace CodeMatcherV2Api.Models
{
    public class EmbeddingModel
    {
        public int Id { get; set; }
        public int RunTypeId { get; set; }
        public int EmbeddingFrequencyId { get; set; }
        public int SegmentTypeId { get; set; }
        public string RunSchedule { get; set; }
    }
}
