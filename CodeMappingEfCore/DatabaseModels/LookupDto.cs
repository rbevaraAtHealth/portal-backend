using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeMappingEfCore.DatabaseModels
{
    public class LookupDto
    {
        public int Id { get; set; }

        public int LookupTypeId { get; set; }

        [ForeignKey("LookupTypeId")]
        public LookupTypeDto LookupType { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
