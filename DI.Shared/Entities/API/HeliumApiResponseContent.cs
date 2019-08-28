using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DI.Shared.Entities.API
{
    public class HeliumApiResponseContent<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        private object _rawMessage { get; set; }

        [JsonIgnoreAttribute]
        private List<string> _messages;

        [JsonIgnoreAttribute]
        protected List<string> Messages
        {
            get
            {
                if (_messages == null)
                {
                    _messages = new List<string>(10);
                    if (_rawMessage is string)
                    {
                        _messages.Add((String)_rawMessage);
                    }
                    else if (_rawMessage is JArray)
                    {
                        foreach (var item in ((JArray)_rawMessage).Values())
                        {
                            if (item is JProperty)
                            {
                                string name = ((JProperty)item).Name;
                                var rawValue = ((JProperty)item).Value;
                                if (rawValue is JArray)
                                {
                                    _messages.AddRange(((JArray)rawValue).ToObject<List<string>>().Select(value => $"{name}: {value}"));
                                }
                                else
                                {
                                    _messages.Add(rawValue.ToString());
                                }
                            }
                            else
                            {
                                _messages.Add(item.ToString());
                            }
                        }
                    }
                    else if (_rawMessage is JObject)
                    {
                        foreach (var property in ((JObject)_rawMessage))
                        {
                            if (property.Value is JArray)
                            {
                                if (!String.Equals(property.Key, "non_field_errors", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    _messages.AddRange(((JArray)property.Value).ToObject<List<string>>().Select(value => $"{property.Key}: {value}"));
                                }
                                else
                                {
                                    _messages.AddRange(((JArray)property.Value).ToObject<List<string>>());
                                }
                            }
                            else
                            {
                                if (!String.Equals(property.Key, "non_field_errors", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    _messages.Add($"{property.Key}: {property.Value}");
                                }
                                else
                                {
                                    _messages.Add($"{property.Value}");
                                }
                            }
                        }
                    }
                    else
                    {
                        _messages.Add(_rawMessage.ToString());
                    }
                }
                return _messages;
            }
        }

        [JsonIgnoreAttribute]
        public string Message
        {
            get
            {
                return String.Join(Environment.NewLine, Messages.ToArray());
            }
        }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
