using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class GeoAddress
    {
        [JsonProperty("place_id")]
        public int PlaceId { get; set; }

        [JsonProperty("licence")]
        public string Licence { get; set; }

        //[JsonProperty("osm_type")]
        //public string OsmType { get; set; }

        //[JsonProperty("osm_id")]
        //public int OsmId { get; set; }

        [JsonProperty("lat")]
        public float Lat { get; set; }

        [JsonProperty("lon")]
        public float Lon { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("address")]
        public Address AddressDetais { get; set; }
    }

    public class Address
    {
        [JsonProperty("bus_station")]
        public string BusStation { get; set; }

        [JsonProperty("road")]
        public string Road { get; set; }

        [JsonProperty("suburb")]
        public string Suburb { get; set; }

        [JsonProperty("city_district")]
        public string CityDistrict { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
    }
}
