using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class TokenResponseModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
