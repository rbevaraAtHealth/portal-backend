using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMappingEfCore.DatabaseModels
{
    public class CodeMappingDto
    {
        [Key]
        public int Id { get; set; }

        public int RequestId { get; set; }

        [ForeignKey("RequestId")]
        public CodeMappingRequestDto? Request { get; set; }

        public string Reference { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string Progress { get; set; } = null!;
    }
}
