using System.ComponentModel.DataAnnotations;

namespace CodeMatcherV2Api.Dtos
{
    public class LookupTypeDto
    {
        [Key]
        public int LookupTypeId { get; set; }
        public string LookupTypeKey { get; set; }
        public string LookupTypeDescription { get; set; }
    }
}
