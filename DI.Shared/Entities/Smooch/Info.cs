using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.Smooch
{
    public class Info
    {
        [JsonProperty("appName")]
        public string AppName { get; set; }

        [JsonProperty("devicePlatform")]
        public string DevicePlatform { get; set; }

        [JsonProperty("os")]
        public string OS { get; set; }

        [JsonProperty("osVersion")]
        public string OSVersion { get; set; }
    }
}
