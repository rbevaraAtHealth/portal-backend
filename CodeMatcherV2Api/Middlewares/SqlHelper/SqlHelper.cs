using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using System.Net.Http;
using System.Net;
using System.Linq;

namespace CodeMatcherV2Api.Middlewares.SqlHelper
{
    public static class SqlHelper
    {
        public static int SaveCodeMappingRequest(CodeMappingRequestDto cgReqModel, CodeMatcherDbContext context)
        {
            context.CodeMappingRequests.Add(cgReqModel);
            context.SaveChanges();
            return (cgReqModel.Id);
        }
        public static void SaveResponseseMessage(CodeMappingResponseDto responseDto, int requestId, CodeMatcherDbContext context)
        {

            context.CodeMappingResponses.Add(responseDto);
            context.SaveChanges();
        }
        public static int GetLookupType(int id,CodeMatcherDbContext context)
        {
            var lookup = context.Lookups.FirstOrDefault(x => x.Id == id);
            return lookup.Id;
        }
        public static int GetLookupType(string type, CodeMatcherDbContext context)
        {
            var lookup = context.Lookups.FirstOrDefault(x => x.Name == type);
            return lookup.Id;
        }
    }
}
