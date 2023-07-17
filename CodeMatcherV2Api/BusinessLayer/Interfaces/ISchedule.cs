using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponeModel;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ISchedule
    {
        CGScheduledRunReqModel ApiRequestGet(ScheduleModel schedule);
        CGScheduleRunResModel APiResponseSave(HttpResponseMessage httpResponse);

    }
}
