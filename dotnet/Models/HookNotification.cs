using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePickup.Models
{
    public class HookNotification
    {
        [JsonProperty("Domain")]
        public string Domain { get; set; }

        [JsonProperty("OrderId")]
        public string OrderId { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("LastState")]
        public string LastState { get; set; }

        [JsonProperty("LastChange")]
        public DateTimeOffset LastChange { get; set; }

        [JsonProperty("CurrentChange")]
        public DateTimeOffset CurrentChange { get; set; }

        [JsonProperty("Origin")]
        public Origin Origin { get; set; }
    }

    public class Origin
    {
        [JsonProperty("Account")]
        public string Account { get; set; }

        [JsonProperty("Key")]
        public string Key { get; set; }
    }
}
