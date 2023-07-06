using AutoMapper;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Dtos;
using CodeMatcherV2Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class Trigger : ITrigger
    {
        public async Task<string> GetAllTriggerAsync(string segment)
        {
            return "Job Triggered Successfully";
        }
    }
}
