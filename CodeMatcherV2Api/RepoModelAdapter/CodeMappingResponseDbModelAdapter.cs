using System.Net.Http;
using System.Net;
using CodeMappingEfCore.DatabaseModels;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class CodeMappingResponseDbModelAdapter
    {
        public CodeMappingResponseDto DbResponseModelGet(HttpResponseMessage httpResponse, int requestId)
        {
            CodeMappingResponseDto responseDto = new CodeMappingResponseDto();
            responseDto.RequestId = requestId;
            responseDto.ResponseMessage = httpResponse.Content.ReadAsStringAsync().Result;
            responseDto.IsSuccess = (httpResponse.StatusCode == HttpStatusCode.OK) ? true : false;
            return responseDto;
        }
    }
}
