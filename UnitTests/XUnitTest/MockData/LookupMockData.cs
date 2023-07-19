using CodeMatcherV2Api.Models;

namespace CodeMatcherReview.Test.MockData
{
    public class LookupMockData
    {
        public static List<LookupModel> LookupData(int id)
        {
            return new List<LookupModel> {
                new LookupModel {
                    Id = id,
                    Name= "Test",
                },
                new LookupModel { Id = 2,
                    Name = "Test2", }
            };
        }
        public static LookupTypeModel LookupTypeMockData(string lookupTypeName)
        {
            return new LookupTypeModel
            {
                LookupTypeId = 1,
                LookupTypeKey = lookupTypeName,
                LookupTypeDescription = "Test",
            };
        }

        public static List<LookupModel> EmptyLookupData(int id)
        {
            return new List<LookupModel> { };
        }
        public static LookupTypeModel EmptyLookupTypeMockData(string lookupTypeName)
        {
            return new LookupTypeModel();
        }
    }
}