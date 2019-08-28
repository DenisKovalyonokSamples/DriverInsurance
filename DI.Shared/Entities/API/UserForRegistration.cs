using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class UserForRegistration
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("company")]
        public int CompanyId { get; set; }

        [JsonProperty("refer_code")]
        public string ReferCode { get; set; }

        [JsonProperty("groups")]
        public List<string> Groups { get; set; }
    }
}
