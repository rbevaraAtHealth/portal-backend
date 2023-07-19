using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriggerRunController : BaseController
    {
        private readonly ITrigger _trigger;
        private readonly ResponseViewModel _responseViewModel;

        public TriggerRunController(ITrigger trigger)
        {
            _trigger = trigger;
            _responseViewModel = new ResponseViewModel();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllTrigger(string segment)
        {
            try
            {
                var triggerJob = await _trigger.GetAllTriggerAsync(segment);
                _responseViewModel.Message = segment;
              
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
            }
            return Ok(_responseViewModel);
        }
    }
}
