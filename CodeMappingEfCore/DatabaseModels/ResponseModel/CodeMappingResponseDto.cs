using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMappingEfCore.DatabaseModels
{
    public class CodeMappingResponseDto : AuditEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequestId { get; set; }

        [ForeignKey("RequestId")]
        public CodeMappingRequestDto Request { get; set; }

        public bool IsSuccess { get; set; } = false;

        public string ResponseMessage { get; set; }
    }
}
