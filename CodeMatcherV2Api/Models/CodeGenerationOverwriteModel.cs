using System.Collections.Generic;

namespace CodeMatcherV2Api.Models
{
    public class CodeGenerationOverwriteModel
    {
        public int Id { get; set; }
        public string ResponseReference { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Frm { get; set; }
        public string Too { get; set; }
        public List<CodeGenerationOverwriteHistoryModel> HistoryData { get; set; }
    }
}
