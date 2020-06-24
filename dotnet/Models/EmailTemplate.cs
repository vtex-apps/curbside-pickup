using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePickup.Models
{
    public class EmailTemplate
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("template")]
        public Template Template { get; set; }
    }

    public class Template
    {
        [JsonProperty("Message")]
        public string Message { get; set; }
    }
}
