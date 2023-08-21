namespace CodeMatcher.Api.V2.BusinessLayer
{
    public static class CodeMappingTypeConst
    {
        public const string CodeGeneration = "Code Generation";
        public const string WeeklyEmbeddings = "Weekly Embedding";
        public const string MonthlyEmbeddings = "Monthly Embedding";
    }
    public static class RequestTypeConst
    {
        public const string Triggered = "Triggered";
        public const string Scheduled = "Scheduled";
        public const string UploadCsv = "Upload Csv";
    }
    public static class StatusConst
    {
        public const string Success = "Completed";
        public const string InProgress = "In Progress";
    }
    public static class LookupTypeConst
    {
        public const string RunType = "RunType";
        public const string CodeMapping = "CodeMappingType";
        public const string Segment = "Segment";
    }
}