using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;

namespace CodeMatcherV2Api.Models
{
    public class CgCsvUploadModel
    {
        public string Segment { get; set; }
        public List<float> Threshold { get; set; }
        public string CsvFilePath { get; set; }
    }
}
