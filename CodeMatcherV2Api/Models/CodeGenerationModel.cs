using System.ComponentModel.DataAnnotations;

namespace CodeMatcherV2Api.Models
{
    public class CodeGenerationModel
    {
        public int RequestId { get; set; }
        public int RunTypeId { get; set; }
        public int SegmentId { get; set; }
        public string RunSchedule { get; set; }
        public string Threshold { get; set; }
        public string LatestLink { get; set; }
        public string ClientId { get; set; }
    }
}
