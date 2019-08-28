using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class ResponseResultWithCode
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
    }
}
