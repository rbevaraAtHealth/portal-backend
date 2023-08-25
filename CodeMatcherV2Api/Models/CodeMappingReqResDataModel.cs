using CodeMappingEfCore.DatabaseModels;

namespace CodeMatcher.Api.V2.Models
{
    public class CodeMappingReqResDataModel
    {
        public CodeMappingRequestDto CodeMappingRequest { get; set; }
        public CodeMappingResponseDto CodeMappingResponse { get; set; }
        public CodeMappingDto CodeMapping { get; set; }
    }
}
