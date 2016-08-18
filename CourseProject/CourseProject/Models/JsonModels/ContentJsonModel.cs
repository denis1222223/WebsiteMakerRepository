using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CourseProject.Models.JsonModels
{
    [JsonObject]
    public class ContentJsonModel
    {
        public ContentJsonModel()
        {
            Content = new List<ContentItemJsonModel>();
        }

        [JsonProperty("content_template")]
        public string ContentTemplate { get; set; }

        [JsonProperty("content")]
        public List<ContentItemJsonModel> Content { get; set; }
    }
}