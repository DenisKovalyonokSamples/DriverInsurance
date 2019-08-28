using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.Smooch
{
    public class MessageModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class MessageResponse
    {
        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("conversation")]
        public Conversation Conversation { get; set; }
    }

    public class MessagesResponse
    {
        [JsonProperty("conversation")]
        public Conversation Conversation { get; set; }

        [JsonProperty("messages")]
        public List<Message> Messages { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }
    }
}
