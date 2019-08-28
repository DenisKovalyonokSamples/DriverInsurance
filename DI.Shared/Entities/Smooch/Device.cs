using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.Smooch
{
    public class Device
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        //[JsonProperty("lastSeen")]
        //public DateTime LastSeen { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("appVersion")]
        public string AppVersion { get; set; }

        //[JsonProperty("active")]
        //public bool Active { get; set; }

        //[JsonProperty("primary")]
        //public bool Primary { get; set; }
    }
}
