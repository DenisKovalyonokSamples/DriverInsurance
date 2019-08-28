using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class DictionariesContainer
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("data")]
        public List<Dictionary> Data { get; set; }
    }
}
