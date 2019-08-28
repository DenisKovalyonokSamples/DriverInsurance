using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class Quotation
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("driver_age")]
        public int DriverAge { get; set; }

        [JsonProperty("driver_exp")]
        public int DriverExp { get; set; }

        [JsonProperty("car")]
        public int Car { get; set; }

        [JsonProperty("client")]
        public int Client { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public int Deleted { get; set; }
    }
}
