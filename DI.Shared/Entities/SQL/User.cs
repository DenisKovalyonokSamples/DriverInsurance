using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DI.Shared.Entities.SQL
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Salt { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public string DeviceUID { get; set; }

        public string UserId { get; set; }

        public int PINAttemptsCount { get; set; }

        public bool IsPhoneVerified { get; set; }
    }
}
