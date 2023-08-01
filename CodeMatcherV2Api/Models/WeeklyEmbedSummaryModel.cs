using CodeMappingEfCore.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace CodeMatcher.Api.V2.Models
{
    public class WeeklyEmbedSummaryModel
    {
        public int Id { get; set; }
        public Guid TaskId { get; set; }
        public int RequestId { get; set; }
      //  public CodeMappingRequestDto CodeMappingRequest { get; set; }
        //public DateTime Date { get; set; }
        public string Date { get; set; }
        public string Segment { get; set; }
        public int NoOfBaseRecordsImportedFromDatabase { get; set; }
        public int NoOfRecordsEmbeddingsCreated { get; set; }
        public int NoOfBaseRecordsBeforeRun { get; set; }
        public int NoOfRecordsAfterRun { get; set; }
        public int StartLink { get; set; }
        public int LatestLink { get; set; }
    }
}
