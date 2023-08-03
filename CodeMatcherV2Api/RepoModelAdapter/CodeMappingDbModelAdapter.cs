using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcherV2Api.EntityFrameworkCore;
using Newtonsoft.Json;


namespace CodeMatcher.Api.V2.RepoModelAdapter
{
    public class CodeMappingDbModelAdapter
    {
        public static CodeMappingDto GetCodeMappingModel(CodeMappingResponseDto responseDto)
        {
            CodeMappingDto codeMapping = new CodeMappingDto();
            if (responseDto.IsSuccess)
            {
                codeMapping.RequestId = responseDto.RequestId;
                var apiResponse = JsonConvert.DeserializeObject<ApiResModel>(responseDto.ResponseMessage);
                codeMapping.Reference = apiResponse.Reference;
                codeMapping.Status = "In progress";
                codeMapping.Progress = "60%";
            }
            return codeMapping;
        }
    }
}
