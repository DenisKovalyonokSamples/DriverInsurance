using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.Smooch
{
    public class InitRequestModel
    {
        [JsonProperty("device")]
        public Device Device { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }
    }

    public class InitResponceModel
    {
        [JsonProperty("app")]
        public App App { get; set; }

        [JsonProperty("appUser")]
        public AppUser AppUser { get; set; }
    }
}
