using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class Loss
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("incident")]
        public int Incident { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("payment_date")]
        public DateTime? PaymentDate { get; set; }

        [JsonProperty("incident_date")]
        public DateTime IncidentDate { get; set; }

        [JsonProperty("place_inspection")]
        public string PlaceInspection { get; set; }

        [JsonProperty("date_inspection")]
        public string DateInspection { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("regressive")]
        public int Regressive { get; set; }

        [JsonProperty("contract")]
        public int Contract { get; set; }

        [JsonProperty("incident_type")]
        public string IncidentType { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public int Deleted { get; set; }
    }
}
