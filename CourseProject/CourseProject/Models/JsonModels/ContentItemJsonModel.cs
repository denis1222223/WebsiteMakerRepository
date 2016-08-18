using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CourseProject.Models.JsonModels
{
    [JsonObject]
    public class ContentItemJsonModel
    {
        [JsonProperty("place")]
        public string Place { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}