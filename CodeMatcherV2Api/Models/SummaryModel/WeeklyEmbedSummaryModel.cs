using CodeMappingEfCore.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CodeMatcher.Api.V2.Models.SummaryModel
{
    public class WeeklyEmbedSummaryModel
    {
        public int Id { get; set; }
        public Guid TaskId { get; set; }
        public int RequestId { get; set; }
        public string Date { get; set; }
        public string Segment { get; set; }
        [JsonProperty(PropertyName = "No_New_BaseRecords_Imported_From_Database")]
        public int NoOfBaseRecordsImportedFromDatabase { get; set; }

        [JsonProperty(PropertyName = "No_of_Records_Embeddings_Created")]
        public int NoOfRecordsEmbeddingsCreated { get; set; }

        [JsonProperty(PropertyName = "No_of_BaseRecords_before_Run")]
        public int NoOfBaseRecordsBeforeRun { get; set; }

        [JsonProperty(PropertyName = "No_of_BaseRecords_after_Run")]
        public int NoOfRecordsAfterRun { get; set; }

        [JsonProperty(PropertyName = "Start_Link")]
        public int StartLink { get; set; }

        [JsonProperty(PropertyName = "Latest_Link")]
        public int LatestLink { get; set; }
    }
}
