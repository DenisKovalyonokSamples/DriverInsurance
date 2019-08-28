using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class BonusTransactionContainer
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public BonusTransaction Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
