using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.Smooch
{
    public class AppUser
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("signedUpAt")]
        public DateTime SignedUpAt { get; set; }

        [JsonProperty("conversationStarted")]
        public bool ConversationStarted { get; set; }

        [JsonProperty("credentialRequired")]
        public bool CredentialRequired { get; set; }

        [JsonProperty("devices")]
        List<Device> Devices { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("clients")]
        List<Client> Clients { get; set; }

        [JsonProperty("pendingClients")]
        public List<Client> PendingClients { get; set; }

        //[JsonProperty("properties")]
        //public string[] Properties { get; set; }
    }

    public class AppUserData
    {
        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
