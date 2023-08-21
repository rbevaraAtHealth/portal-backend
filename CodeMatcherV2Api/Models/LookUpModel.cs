using CodeMappingEfCore.DatabaseModels;
using System.Collections.Generic;

namespace CodeMatcherV2Api.Models
{
    public class LookupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LookupTypeId { get; set; }
        public LookupTypeDto LookupType { get; set; }
    }
}
