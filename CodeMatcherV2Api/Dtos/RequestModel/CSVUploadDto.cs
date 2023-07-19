using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMatcherV2Api.Dtos.RequestModel
{
    public class CSVUploadDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SegmentTypeId { get; set; }

        [ForeignKey("SegmentTypeId")]
        public LookupDto SegmentType { get; set; }

        public string Threshold { get; set; }
    }
}
