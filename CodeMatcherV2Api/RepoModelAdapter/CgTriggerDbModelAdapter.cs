using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponeModel;
using CodeMatcherV2Api.BusinessLayer.Enums;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using CodeMatcher.Api.V2.BusinessLayer.Enums;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class CgTriggerDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, CgTriggeredRunReqModel>
    {
        public CodeMappingRequestDto RequestModel_Get(CgTriggeredRunReqModel pyAPIModel, RequestType type,CodeMappingType codeMappingType, CodeMatcherDbContext context)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            var lookup = context.Lookups.FirstOrDefaultAsync(x => x.Id == (int)type).Result;
            var runTypeId = lookup.Id;
            var segment = context.Lookups.FirstOrDefaultAsync(x => x.Name == pyAPIModel.Segment).Result;
            var segmentId = segment.Id;
            var codemapping = context.Lookups.FirstOrDefaultAsync(x => x.Id == (int)codeMappingType).Result;

            codeMappingRequestDto.RunTypeId = runTypeId;
            codeMappingRequestDto.SegmentTypeId = segmentId;
            codeMappingRequestDto.CodeMappingId = codemapping.Id;
            codeMappingRequestDto.Threshold = pyAPIModel.Threshold.ToString();
            codeMappingRequestDto.LatestLink = pyAPIModel.LatestLink;
            codeMappingRequestDto.ClientId = pyAPIModel.ClientId;
            return codeMappingRequestDto;
        }

        //public CodeMappingResponseDto ResponseModel_Get(HttpResponseMessage httpResponse, int requestId,CodeMatcherDbContext context)
        //{
        //    CodeMappingResponseDto responseDto = new CodeMappingResponseDto();
        //    responseDto.RequestId = requestId;
        //    responseDto.IsSuccess=(httpResponse.IsSuccessStatusCode)?true: false;
        //    responseDto.ResponseMessage= httpResponse.Content.ReadAsStringAsync().Result;
        //    return responseDto;
        //}
    }
}
