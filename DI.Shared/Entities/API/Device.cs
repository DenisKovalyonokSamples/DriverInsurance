using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class Device
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("serialnumber")]
        public string SerialNumber { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("firmware")]
        public string Firmware { get; set; }

        [JsonProperty("profile")]
        public string Profile { get; set; }

        [JsonProperty("imei")]
        public string Imei { get; set; }

        [JsonProperty("imei2")]
        public string Imei2 { get; set; }

        [JsonProperty("stock")]
        public int? StockId { get; set; }

        [JsonProperty("imei_count")]
        public int ImeiCount { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public int Deleted { get; set; }
    }
}
