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
        public CodeGenerationOverwriteDto CodeGenerationOverwrite { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
