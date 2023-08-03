namespace CodeMatcher.Api.V2.Models
{
    public class ScheduleJobModel
    {
        public string Segment { get; set; }
        public float Threshold { get; set; }
        public string CronExpression { get; set; }
    }
}
