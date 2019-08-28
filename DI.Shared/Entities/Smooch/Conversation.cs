using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.Smooch
{
    public class Conversation
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("unreadCount")]
        public int UnreadCount { get; set; }
    }
}
