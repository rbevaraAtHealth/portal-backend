﻿using CodeMatcherV2Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ITrigger
    {
        Task<string> GetAllTriggerAsync(string segment);
    }
}