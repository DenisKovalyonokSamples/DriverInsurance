using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class Car
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("vin")]
        public string Vin { get; set; }

        [JsonProperty("regnum")]
        public string Regnum { get; set; }

        [JsonProperty("device")]
        public int? Device { get; set; }

        [JsonProperty("owner")]
        public int Owner { get; set; }

        [JsonProperty("installation_date")]
        public DateTime? InstallationDate { get; set; }

        [JsonProperty("sim")]
        public int? Sim { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public int Deleted { get; set; }

        [JsonProperty("last_pos_lat")]
        public float? PosEndLat { get; set; }

        [JsonProperty("last_pos_lng")]
        public float? PosEndLng { get; set; }
    }
}
