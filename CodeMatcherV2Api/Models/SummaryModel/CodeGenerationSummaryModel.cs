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
        public string TaskId { get; set; }

        [JsonPropertyName("Date")]
        public string Date { get; set; }
        public string Segment { get; set; } 
        public string Threshold { get; set; } 

        [JsonProperty(PropertyName = "No_of_BaseRecords")]
        public int NoOfBaseRecords { get; set; }

        [JsonProperty(PropertyName = "No_of_Input_Records")]
        public int NoOfInputRecords { get; set; }

        [JsonProperty(PropertyName = "Number_of_Noisy_Records")]
        public int NoOfNoiseRecords { get; set; }

        [JsonProperty(PropertyName = "Number_of_Processed_Records")]

        public int NoOfProcessedRecords { get; set; }

        [JsonProperty(PropertyName = "Number_of_Records_for_which_code_generated")]
        public int NoOfRecordsForWhichCodeGenerated { get; set; }

        [JsonProperty(PropertyName = "Number_of_Records_for_which_code_not_generated")]
        public int NoOfRecordsForWhichCodeNotGenerated { get; set; }

        [JsonProperty(PropertyName = "Start_Link")]
        public string StartLink { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "Latest_Link")]
        public string LatestLink { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "Client_Id")]
        public string ClientId { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "Output_Dir_Path")]
        public string UploadCsvOutputDirPath { get; set; } = string.Empty;
    }
}
