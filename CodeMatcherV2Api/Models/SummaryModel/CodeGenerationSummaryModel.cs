using CodeMappingEfCore.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using AutoMapper.Execution;

namespace CodeMatcher.Api.V2.Models.SummaryModel
{
    public class CodeGenerationSummaryModel
    {
        public Guid TaskId { get; set; }
        // public int RequestId { get; set; }
        //public CodeMappingRequestDto CodeMappingRequest { get; set; }
        //public DateTime Date { get; set; }
        [JsonPropertyName("Date")]
        public string Date { get; set; }
        public string Segment { get; set; }
        public float Threshold { get; set; }

        [JsonProperty(PropertyName = "No_of_BaseRecords")]
        public int NoOfBaseRecords { get; set; }

        [JsonProperty(PropertyName = "No_of_Input_Records")]
        public int NoOfInputRecords { get; set; }

        [JsonProperty(PropertyName = "Number of Noisy Records")]
        public int NoOfNoiseRecords { get; set; }

        [JsonProperty(PropertyName = "Number of Processed Records")]

        public int NoOfProcessedRecords { get; set; }

        [JsonProperty(PropertyName = "Number of Records for which code generated")]
        public int NoOfRecordsForWhichCodeGenerated { get; set; }

        [JsonProperty(PropertyName = "Number of Records for which code not generated")]
        public int NoOfRecordsForWhichCodeNotGenerated { get; set; }

        [JsonProperty(PropertyName = "Start_Link")]
        public string StartLink { get; set; }

        [JsonProperty(PropertyName = "Latest_Link")]
        public string LatestLink { get; set; }
        [JsonProperty(PropertyName = "Client_Id")]
        public string ClientId { get; set; }
    }
}
