using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class TrackingDay
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("car")]
        public int Car { get; set; }

        [JsonProperty("day")]
        public DateTime Day { get; set; }

        [JsonProperty("trackparam")]
        public int Trackparam { get; set; }

        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("trackparam_code")]
        public string TrackparamCode { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public int Deleted { get; set; }
    }
}
