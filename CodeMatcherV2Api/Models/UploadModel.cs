using Microsoft.AspNetCore.Http;
using System.IO;

namespace CodeMatcherV2Api.Models
{
    public class UploadModel
    {
        public string Segment { get; set; }
        public float Threshold { get; set; }
        public string CsvFilePath { get; set; }
    }
}
