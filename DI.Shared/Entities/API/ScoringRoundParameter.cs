using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class ScoringRoundParameter
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("round")]
        public int Round { get; set; }

        [JsonProperty("parameter")]
        public int Parameter { get; set; }

        [JsonProperty("parameter_code")]
        public string ParameterCode { get; set; }

        [JsonProperty("criteria")]
        public int? Criteria { get; set; }

        [JsonProperty("point")]
        public float Point { get; set; }

        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public int Deleted { get; set; }
    }
}
