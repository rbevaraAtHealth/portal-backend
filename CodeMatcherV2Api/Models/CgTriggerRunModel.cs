using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CodeMatcherV2Api.Models
{
    public class CgTriggerRunModel
    {
        public string Segment { get; set; }
        public string Threshold { get; set; }
    }
}
