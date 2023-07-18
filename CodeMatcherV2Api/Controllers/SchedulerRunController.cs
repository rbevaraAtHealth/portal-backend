using CodeMatcherV2Api.BusinessLayer;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CodeMatcherV2Api.Models;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerRunController : BaseController
    {
        private readonly ISchedule _schedule;
        private readonly ResponseViewModel _responseViewModel;
        public SchedulerRunController(ISchedule schedule)
        {
            _schedule = schedule;
            _responseViewModel = new ResponseViewModel();
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleJob([FromBody] ScheduleModel schedule)
        {

            try
            {
                var scheduleJob= await _schedule.ScheduleJobAsync(schedule);
                _responseViewModel.Message = scheduleJob;
            
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
            }
            return Ok(_responseViewModel);
        }
    }
}