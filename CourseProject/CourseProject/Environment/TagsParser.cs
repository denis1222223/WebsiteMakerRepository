using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace CourseProject.Environment
{
    public static class TagsParser
    {
        private static string template = @"#([a-zёа-я0-9_]{1,20})";
        private static Regex TagsRegex = new Regex(template);

        public static string[] Parse(String inputString)
        {
            List<string> parsedTags = new List<string>();
            inputString = inputString.ToLower();
            foreach (Match tagMatch in TagsRegex.Matches(inputString))
            {
                parsedTags.Add(tagMatch.Groups[1].Value);
            }
            return parsedTags.ToArray();
        }
    }
}