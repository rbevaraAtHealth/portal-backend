using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMatcherV2Api.Dtos
{
    public class LookupDto
    {
        public int Id { get; set; }

        public int LookupTypeId { get; set; }

        [ForeignKey("LookupTypeId")]
        public LookupTypeDto LookupTypeDto { get; set; }

        public string Name { get; set; }
    }
}
