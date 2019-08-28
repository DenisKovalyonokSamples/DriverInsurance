using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class StatusValue
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
