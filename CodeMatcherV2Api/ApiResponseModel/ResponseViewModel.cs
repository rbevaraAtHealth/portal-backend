using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeMatcher.Api.V2.ApiResponseModel
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
