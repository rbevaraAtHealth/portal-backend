using System.ComponentModel.DataAnnotations;

namespace CodeMappingEfCore.DatabaseModels
{
    public class LookupTypeDto
    {
        [Key]
        public int LookupTypeId { get; set; }
        public string LookupTypeKey { get; set; } = null!;
        public string LookupTypeDescription { get; set; } = null!;
    }
}
