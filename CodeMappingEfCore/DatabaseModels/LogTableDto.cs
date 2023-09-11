using CodeMappingEfCore.DatabaseModels;
using System.ComponentModel.DataAnnotations;

namespace CodeMatcher.EntityFrameworkCore.DatabaseModels
{
    public class LogTableDto: AuditEntity
    {
        [Key]
        public int Id { get; set; }
        public string LogName { get; set; }
        public string LogDescription { get; set; }
    }
}
