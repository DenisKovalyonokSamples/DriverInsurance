using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.Smooch
{
    public class JWTModel
    {
        [JsonProperty("jwt")]
        public string Jwt { get; set; }
    }
}
