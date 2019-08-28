using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class ScoringRound
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("scformat")]
        public int ScFormat { get; set; }

        [JsonProperty("format_version")]
        public int FormatVersion { get; set; }

        [JsonProperty("contract")]
        public int Contract { get; set; }

        [JsonProperty("schedule")]
        public int? Schedule { get; set; }

        [JsonProperty("format_code")]
        public string FormatCode { get; set; }

        [JsonProperty("points_moment")]
        public float PointsMoment { get; set; }

        [JsonProperty("points")]
        public float Points { get; set; }

        [JsonProperty("score_day")]
        public DateTime ScoreDay { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public int Deleted { get; set; }
    }
}
