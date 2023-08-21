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
        public string ResponseReference { get; set; } = null!;
        public string SerialNumber { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Link { get; set; } = null!;
        public string Frm { get; set; } = null!;
        public string Too { get; set; } = null!;
        public List<CodeGenerationOverwriteHistoryDto> HistoryData { get; set; } = null!;

    }
}
