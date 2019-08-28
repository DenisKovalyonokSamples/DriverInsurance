using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class TrackingTrip
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("car")]
        public int Car { get; set; }

        [JsonProperty("time_end")]
        public int? TimeEnd { get; set; }

        [JsonProperty("time_start")]
        public int TimeStart { get; set; }

        [JsonProperty("packets_count")]
        public int PacketsCount { get; set; }

        [JsonProperty("distance")]
        public int Distance { get; set; }

        [JsonProperty("duration")]
        public int? Duration { get; set; }

        [JsonProperty("max_speed")]
        public float MaxSpeed { get; set; }

        [JsonProperty("avg_speed")]
        public float AvgSpeed { get; set; }

        [JsonProperty("pos_begin_lat")]
        public float? PosBeginLat { get; set; }

        [JsonProperty("pos_begin_lng")]
        public float? PosBeginLng { get; set; }

        [JsonProperty("pos_end_lat")]
        public float? PosEndLat { get; set; }

        [JsonProperty("pos_end_lng")]
        public float? PosEndLng { get; set; }

        [JsonProperty("trip_type")]
        public string TripType { get; set; }

        [JsonProperty("trip")]
        public int? Trip { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public int Deleted { get; set; }

        [JsonProperty("geo_address_begin")]
        public string AddressStart { get; set; }

        [JsonProperty("geo_address_end")]
        public string AddressEnd { get; set; }

        public GeoAddress AddressStartDetails
        {
            get
            {
                if (!string.IsNullOrEmpty(AddressStart))
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
                    };

                    return JsonConvert.DeserializeObject<GeoAddress>(AddressStart, settings);
                }

                return null;
            }
        }

        public GeoAddress AddressEndDetails
        {
            get
            {
                if (!string.IsNullOrEmpty(AddressEnd))
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
                    };

                    return JsonConvert.DeserializeObject<GeoAddress>(AddressEnd, settings);
                }

                return null;
            }
        }
    }
}
