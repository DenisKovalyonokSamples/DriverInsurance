using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class TrackingTripsContainer
    {
        [JsonProperty("data")]
        public List<TrackingTrip> Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
