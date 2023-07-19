using System;

namespace CodeMatcherV2Api.Models
{
    public class CodeMappingModel
    {
        public int Id { get; set; }
        public DateTime TimeStamp   { get; set; }
        public string Segment { get; set; }
        public int NumberOfRecord { get; set; }
        public int NumberOfMatches { get; set; }
        public string RunBy { get; set; }
        public string RunType { get; set; }
        public CodeMappingSummary CodeMappingSummary { get; set; }
    }
}
