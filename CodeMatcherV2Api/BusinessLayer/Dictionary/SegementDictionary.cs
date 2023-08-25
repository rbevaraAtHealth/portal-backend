using System;
using System.Collections.Generic;

namespace CodeMatcher.Api.V2.BusinessLayer.Dictionary
{
    public static class SegmentDictionary
    {
        public static Dictionary<string, string> SegmentsMappings()
        {
            Dictionary<string, string> segment = new Dictionary<string, string>();
            segment.Add("School", "School");
            segment.Add("Hospital", "Hospital");
            segment.Add("Insurance", "Insur");
            segment.Add("State License", "Statelic");
            return segment;
        }

        public static string GetSegmentValueByKey(string key)
        {
            Dictionary<string, string> segmentMappings = SegmentsMappings();

            if (segmentMappings.TryGetValue(key, out string value))
            {
                return value;
            }

            return "Key not found";
        }
    }
}
