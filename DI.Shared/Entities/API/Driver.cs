using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class Driver
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("dob")]
        public DateTime Dob { get; set; }

        [JsonProperty("license_series")]
        public string LicenseSeries { get; set; }

        [JsonProperty("license_date")]
        public DateTime LicenseDate { get; set; }

        [JsonProperty("license_number")]
        public string LicenseNumber { get; set; }
    }
}
