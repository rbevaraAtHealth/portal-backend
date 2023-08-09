using System;

namespace CodeMatcherV2Api.Models
{
    public class CodeMappingSummary
    {
        public int Id { get; set; }
        public int RecordParticulars { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Segment_type { get; set; }
        public string Threshhold { get; set;}
        public int BaseRecords { get; set; }
        public int InputRecords { get; set; }
        public int NoisyRecords { get; set; }
        public int ProcessedRecords { get; set; }
        public int CodeGeneratedRecords { get; set; }
        public int CodeNotGeneratedRecords { get; set; }
    }
}
