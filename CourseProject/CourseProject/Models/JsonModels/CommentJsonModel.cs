using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CourseProject.Models.Entities;

namespace CourseProject.Models.JsonModels
{
    public class CommentJsonModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("author_name")]
        public string AuthorName { get; set; }

        [JsonProperty("author_picture")]
        public string AuthorPicture { get; set; }

        [JsonProperty("date_time")]
        public string DateTime { get; set; }
    }
}