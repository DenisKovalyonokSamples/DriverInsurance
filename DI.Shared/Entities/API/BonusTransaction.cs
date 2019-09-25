using Newtonsoft.Json;
using System;

namespace DI.Shared.Entities.API
{
    public class BonusTransaction
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("correction")]
        public int Correction { get; set; }

        [JsonProperty("transaction_type")]
        public string TransactionType { get; set; }

        [JsonProperty("contract")]
        public int Contract { get; set; }

        [JsonProperty("bonus")]
        public int? Bonus { get; set; }

        [JsonProperty("user")]
        public int? User { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public int Deleted { get; set; }
    }
}
