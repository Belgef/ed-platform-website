using System.Text.RegularExpressions;

namespace EdPlatformWebsite.CodeExecution
{
    public class OutputMatcher
    {
        private readonly string _pattern;

        public OutputMatcher(string pattern)
        {
            _pattern = pattern;
        }

        public readonly string NewLine = "^\r?\n\r?";
        public readonly string Number = @"^-?\d+";
        public readonly string Decimal = @"^-?\d+([.,]\d+)?";

        public List<object?> GetObjects(string str)
        {
            List<object?> objects = new List<object?>();
            using StringReader reader = new StringReader(str);
            string? line = reader.ReadLine();
            foreach (char c in _pattern)
            {
                switch (c)
                {
                    case 'd':
                        if (line==null)
                            objects.Add(null);
                        else
                        {
                            line = line.TrimStart();
                            string resultStr = Regex.Match(line, Number).Value;
                            line = Regex.Replace(line, Number+@"\s*", "");
                            bool success = int.TryParse(resultStr, out int resultObj);
                            objects.Add(success?resultObj:null);
                        }
                        break;
                    case 'f':
                        if (line==null)
                            objects.Add(null);
                        else
                        {
                            line = line.TrimStart();
                            string resultStr = Regex.Match(line, Decimal).Value;
                            line =  Regex.Replace(line, Decimal+@"\s*", "");
                            bool success = double.TryParse(resultStr.Replace('.', ','), out double resultObj);
                            objects.Add(success ? resultObj : null);
                        }
                        break;
                    case 'n':
                        if (Regex.Match(line, @"\S").Success)
                            throw new ArgumentException("Wrong input");
                        line = reader.ReadLine();
                        break;
                    default:
                        throw new ArgumentException("Output matcher pattern contains unrecognized character");
                }
            }
            if (Regex.Match(line, @"\S").Success)
                throw new ArgumentException("Wrong input");
            return objects;
        }
    }
}
