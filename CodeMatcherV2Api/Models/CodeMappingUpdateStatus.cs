namespace CodeMatcher.Api.V2.Models
{
    public class CodeMappingUpdateStatus
    {
        public string TaskId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Progress { get; set; }
    }
}
