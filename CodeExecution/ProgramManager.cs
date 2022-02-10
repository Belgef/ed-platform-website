﻿using EdPlatformWebsite.Models;
using System.Net;

namespace EdPlatformWebsite.CodeExecution
{
    public class ProgramManager
    {
        private readonly string _code;
        private readonly Uri _executionUri;
        private readonly OutputMatcher _matcher;

        public ProgramManager(string code, Uri executionUri, string outputPattern)
        {
            _code = code;
            _executionUri = executionUri;
            _matcher = new OutputMatcher(outputPattern);
        }
        public string Check(IOCase iocase)
        {
            string output = GetOutput(iocase.Input ?? "", out string? error);
            if(error != null)
                return error;
            List<object?> expected = _matcher.GetObjects(iocase.Output ?? "");
            List<object?> actual = _matcher.GetObjects(output);
            for(int i = 0; i < expected.Count; i++)
            {
                if (expected[i] == null || actual[i] == null)
                    return "failed";
                else if (expected[i] is double d1 && actual[i] is double d2 && d1 - d2 > 1e-5)
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
                output = wc.DownloadString($"https://rest-api-ed-platform.herokuapp.com/?code={_code}&inputString={input}");
            }
            var results = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(output);
            error = results["error"]?.ToString();
            return results["output"]?.ToString()??"";
        }
    }
}