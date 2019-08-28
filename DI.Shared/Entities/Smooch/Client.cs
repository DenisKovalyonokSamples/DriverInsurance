using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.Smooch
{
    public class Client
    {
        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("appVersion")]
        public string AppVersion { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("lastSeen")]
        public DateTime LastSeen { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("pushNotificationToken")]
        public string PushNotificationToken { get; set; }

        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("primary")]
        public bool Primary { get; set; }
    }
}
