using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMappingEfCore.DatabaseModels
{
    public class CodeGenerationOverwriteDto: AuditEntity
    {
        [Key]
        public int Id { get; set; }
        public string ResponseReference { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Frm { get; set; }
        public string Too { get; set; }
        public List<CodeGenerationOverwriteHistoryDto> HistoryData { get; set; }

    }
}
