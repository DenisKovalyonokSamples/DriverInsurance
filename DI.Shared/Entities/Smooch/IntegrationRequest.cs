using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.Smooch
{
    public class IntegrationRequest
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("serverKey")]
        public string ServerKey { get; set; }

        [JsonProperty("senderId")]
        public string SenderId { get; set; }
    }
}
