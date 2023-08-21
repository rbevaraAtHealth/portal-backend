using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMappingEfCore.DatabaseModels
{
    public class CSVUploadDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SegmentTypeId { get; set; }

        [ForeignKey("SegmentTypeId")]
        public LookupDto SegmentType { get; set; } = null!;

        public string Threshold { get; set; } = null!;
    }
}
