using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.API
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("id_smooch")]
        public string SmoochId { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }

        [JsonProperty("middlename")]
        public string MiddleName { get; set; }

        [JsonProperty("user_code")]
        public string UserCode { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("password_expire")]
        public DateTime? PasswordExpire { get; set; }

        [JsonProperty("company")]
        public int? CompanyId { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("is_admin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("deleted")]
        public int Deleted { get; set; }

        [JsonProperty("is_password_expired")]
        public bool IsPasswordExpired { get; set; }

        [JsonProperty("user_permissions")]
        public List<string> UserPermissions { get; set; }

        [JsonProperty("groups")]
        public List<string> Groups { get; set; }
    }

    public class UserForUpdate
    {
        [JsonProperty("id_smooch")]
        public string SmoochId { get; set; }
    }
}
