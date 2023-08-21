using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMappingEfCore.DatabaseModels
{
    public class CodeGenerationOverwriteHistoryDto : AuditEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("OverWriteID")]
        public CodeGenerationOverwriteDto CodeGenerationOverwrite { get; set; } = null!;
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
    }
}
