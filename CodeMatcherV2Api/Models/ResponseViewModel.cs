using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeMatcherV2Api.Models
{
    public class ResponseViewModel
    {
        string _exceptionMessage;
        public object Model { get; set; }
        public IEnumerable<ValidationResult> ValidationResults { get; set; }
        public string ExceptionMessage
        {
            get { return _exceptionMessage; }
            set
            {
                _exceptionMessage = value;
            }
        }
        public string Message { get; set; }
    }
}
