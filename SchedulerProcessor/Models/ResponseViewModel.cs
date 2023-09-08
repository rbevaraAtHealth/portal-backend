using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchedulerProcessor.Models
{
    public class ResponseViewModel
    {
        public object Model { get; set; }
        public IEnumerable<ValidationResult> ValidationResults { get; set; }
        public string ExceptionMessage { get; set; }
        public string Message { get; set; }
        public int RowsAffected { get; set; }
    }
}
