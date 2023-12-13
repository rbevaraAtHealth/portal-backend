using System;

namespace CodeMatcher.Api.V2.Models
{
    public class APIKeyResponseModel
    {
        public string Api_Key { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
