using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CourseProject.Models.JsonModels
{
    [JsonObject]
    public class MenuJsonModel
    {
        public MenuJsonModel()
        {
            HorizontalMenu = new List<MenuItemJsonModel>();
            VerticalMenu = new List<MenuItemJsonModel>();
        }
        [JsonProperty("horizontal_menu_exist")]
        public bool HorizontalMenuExist { get; set; }

        [JsonProperty("horizontal_menu")]
        public List<MenuItemJsonModel> HorizontalMenu { get; set; }

        [JsonProperty("vertical_menu_exist")]
        public bool VerticalMenuExist { get; set; }

        [JsonProperty("vertical_menu")]
        public List<MenuItemJsonModel> VerticalMenu { get; set; }
    }
}