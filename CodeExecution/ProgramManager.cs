using EdPlatformWebsite.Models;
using System.Net;

namespace EdPlatformWebsite.CodeExecution
{
    public class ProgramManager
    {
        public const string Python = "https://rest-api-ed-platform.herokuapp.com/";

        private readonly string _code;
        private readonly string _executionUri;
        private readonly OutputMatcher _matcher;

        public ProgramManager(string code, string executionUri, string outputPattern)
        {
            _code = code;
            _executionUri = executionUri;
            _matcher = new OutputMatcher(outputPattern);
        }
        public string Check(IOCase iocase)
        {
            string output = GetOutput(iocase.Input?.Replace("\r", "") ?? "", out string? error);
            if(error != null && error.Length > 0)
                return error;
            List<object?> expected = _matcher.GetObjects(iocase.Output ?? "");
            List<object?> actual = _matcher.GetObjects(output);
            for(int i = 0; i < expected.Count; i++)
            {
                if (expected[i] == null || actual[i] == null)
                    return "failed";
                else if (expected[i] is double d1 && actual[i] is double d2 && Math.Abs(d1 - d2) > 1e-5)
                    return "failed";
                else if(!expected[i]?.Equals(actual[i]) ?? true)
                    return "failed";
            }
            return "succeed";
        }
        public string GetOutput(string input, out string? error)
        {
            string output;
            using (WebClient wc = new WebClient())
            {
                output = wc.DownloadString(_executionUri+$"?code={System.Web.HttpUtility.UrlEncode(_code)}&inputString={System.Web.HttpUtility.UrlEncode(input)}");
            }
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(output);
            error = results["error"]?.ToString();
            return results["output"]?.ToString()??"";
        }
    }
}
