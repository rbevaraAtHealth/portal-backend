namespace CodeMatcherV2Api
{
    public class ResponseResult
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public class Result
    {
        public Status status { get; set; }
        public object data { get; set; }
    }

    public class Status
    {
        public int code { get; set; }
        public string message { get; set; }
    }
}
