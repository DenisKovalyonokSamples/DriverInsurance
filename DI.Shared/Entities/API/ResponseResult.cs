using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class ResponseResult
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
