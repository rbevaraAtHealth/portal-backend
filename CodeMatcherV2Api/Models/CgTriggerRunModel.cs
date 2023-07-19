using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CodeMatcherV2Api.Models
{
    public class CgTriggerRunModel
    {
        public string Segment { get; set; }
        public float Threshold { get; set; }
    }
}
