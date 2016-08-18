using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CourseProject.Environment
{
    public class Base64Decoder
    {
        public byte[] Decode(string pictureDataUrl)
        {
            var base64Data = Regex.Match(pictureDataUrl, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);
            return binData;
        }
    }
}