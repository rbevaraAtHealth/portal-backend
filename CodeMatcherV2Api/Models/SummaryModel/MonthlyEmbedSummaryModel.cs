using CodeMappingEfCore.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CodeMatcher.Api.V2.Models.SummaryModel
{
    public class MonthlyEmbedSummaryModel
    {
        public int Id { get; set; }
        public string TaskId { get; set; }
        public int RequestId { get; set; }
        public string Date { get; set; }
        public string Segment { get; set; }

        [JsonProperty(PropertyName = "No_BaseRecords_Imported_From_Database")]
        public int NoOfRecordsImportedFromDatabase { get; set; }

        [JsonProperty(PropertyName = "No_of_Records_Embeddings_Created")]
        public int NoOfRecordsEmbeddingCreated { get; set; }
    }
}
