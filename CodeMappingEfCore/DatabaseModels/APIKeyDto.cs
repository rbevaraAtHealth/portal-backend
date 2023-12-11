using CodeMappingEfCore.DatabaseModels;
using System.ComponentModel.DataAnnotations;

namespace CodeMatcher.EntityFrameworkCore.DatabaseModels
{
    public class APIKeyDto : AuditEntity
    {
        [Key]
        public int Id { get; set; }
        public string Api_Key { get; set; }
        public DateTime LastAccessedOn { get; set; }
    }
}
