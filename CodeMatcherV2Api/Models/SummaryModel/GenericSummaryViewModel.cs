using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;

namespace CodeMatcher.Api.V2.Models.SummaryModel
{
    public class GenericSummaryViewModel
    {
       // public int Id { get; set; }
        public string TaskId { get; set; }
        public int RequestId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Segment { get; set; }
        public string CodeMappingType { get; set; }
        public string Threshold { get; set; }
        public int NoOfRecords { get; set; }
        public int NoofMatches { get; set; }
        public string RunBy { get; set; }
        public string RunType { get; set; }
        public object Summary { get; set; }
    }
}
