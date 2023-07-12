﻿using CodeMatcherV2Api.Models;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ILookupTypes
    {
        public  Task<LookupTypeModel> GetLookupByNameAsync(string lookupType);
    }
}