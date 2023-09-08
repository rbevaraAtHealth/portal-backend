using System;

namespace SchedulerProcessor.Models
{
    public class SchedulerModel
    {
        public string ClientId { get; set; }
        public string CodeMapping { get; set; }
        public string Segment { get; set; }
        public string Threshold { get; set; }
        public string CronExpression { get; set; }
    }
}