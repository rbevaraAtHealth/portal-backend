using CodeMatcherV2Api.Dtos.RequestDtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMatcherV2Api.Dtos.ResponseModel
{
    public class CodeGenerationResponseDto : AuditEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RequestId { get; set; }

        [ForeignKey("RequestId")]
        public CodeGenerationRequestDto Request { get; set; }

        public bool IsSuccess { get; set; } = false;

        public string ResponseMessage { get; set; }

        public string Reference { get; set; }
    }
}
