using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class Incident
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("device")]
        public int Device { get; set; }

        [JsonProperty("car")]
        public int Car { get; set; }

        [JsonProperty("contract")]
        public int Contract { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("incident_type")]
        public string IncidentType { get; set; }

        [JsonProperty("incident_date")]
        public DateTime IncidentDate { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("pos_lat")]
        public float? PosLat { get; set; }

        [JsonProperty("pos_lng")]
        public float? PosLgn { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public int Deleted { get; set; }
    }
}
