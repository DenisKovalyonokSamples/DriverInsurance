using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.Smooch
{
    public class App
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }

        [JsonProperty("stripeEnabled")]
        public bool StripeEnabled { get; set; }

        [JsonProperty("hasIcon")]
        public bool HasIcon { get; set; }

        [JsonProperty("apnEnabled")]
        public bool ApnEnabled { get; set; }

        [JsonProperty("gcmProject")]
        public string GcmProject { get; set; }
    }
}
