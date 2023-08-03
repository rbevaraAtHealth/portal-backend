using CodeMatcher.Api.V2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcher.Api.V2.BusinessLayer.Interfaces
{
    public interface IScheduler
    {
        Task<List<SchedulerModel>> GetAllSchedulersAsync();
    }
}