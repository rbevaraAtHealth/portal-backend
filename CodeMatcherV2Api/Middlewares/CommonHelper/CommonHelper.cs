using CodeMatcherApiV2.Common;

namespace CodeMatcher.Api.V2.Middlewares.CommonHelper
{
    public static class CommonHelper
    {
        public static string Decrypt(string encrytedValue)
        {
            Decrypt _decrypt = new Decrypt();
            EncDecModel _res = _decrypt.DecryptString(encrytedValue);
            return _res.outPut;
        }
    }
}
