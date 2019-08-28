using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class Tracking
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("device")]
        public int Device { get; set; }

        [JsonProperty("car")]
        public int Car { get; set; }

        [JsonProperty("time_end")]
        public int TimeEnd { get; set; }

        [JsonProperty("time_start")]
        public int TimeStart { get; set; }

        [JsonProperty("packets_count")]
        public int PacketsCount { get; set; }

        [JsonProperty("distance")]
        public int Distance { get; set; }

        [JsonProperty("max_speed")]
        public float MaxSpeed { get; set; }

        [JsonProperty("avg_speed")]
        public float AvgSpeed { get; set; }

        [JsonProperty("pos_begin_lat")]
        public float? PosBeginLat { get; set; }

        [JsonProperty("pos_begin_lng")]
        public float? PosBeginLng { get; set; }

        [JsonProperty("tzoffset")]
        public int Tzoffset { get; set; }

        [JsonProperty("pos_end_lat")]
        public float PosEndLat { get; set; }

        [JsonProperty("pos_end_lng")]
        public float PosEndLng { get; set; }

        [JsonProperty("tzname")]
        public string Tzname { get; set; }

        [JsonProperty("local_time")]
        public int LocalTime { get; set; }

        [JsonProperty("local_hour")]
        public int LocalHour { get; set; }

        [JsonProperty("local_dow")]
        public int LocalDow { get; set; }

        [JsonProperty("local_month")]
        public int LocalMonth { get; set; }

        [JsonProperty("trip")]
        public int Trip { get; set; }

        [JsonProperty("geo_address")]
        public string GeoAddress { get; set; }

        public GeoAddress GeoAddressDetails
        {
            get
            {
                if (!string.IsNullOrEmpty(GeoAddress))
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings()
                    {
                        TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
                    };

                    return JsonConvert.DeserializeObject<GeoAddress>(GeoAddress, settings);
                }

                return null;
            }
        }

        [JsonProperty("geo_country")]
        public string GeoCountry { get; set; }

        [JsonProperty("geo_region")]
        public string GeoRegion { get; set; }

        [JsonProperty("geo_district")]
        public string GeoDistrict { get; set; }

        [JsonProperty("accel_front")]
        public int AccelFront { get; set; }

        [JsonProperty("accel_lateral")]
        public int AccelLateral { get; set; }

        [JsonProperty("brake")]
        public int Brake { get; set; }

        [JsonProperty("geo_flow_speed")]
        public float GeoFlowSpeed { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public int Deleted { get; set; }
    }
}
