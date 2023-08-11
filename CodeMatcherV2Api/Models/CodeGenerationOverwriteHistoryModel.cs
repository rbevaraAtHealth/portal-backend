using CodeMappingEfCore.DatabaseModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CodeMatcherV2Api.Models
{
    public class CodeGenerationOverwriteHistoryModel
    {
        public int Id { get; set; }
        public int CodeGenerationOverwriteId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
